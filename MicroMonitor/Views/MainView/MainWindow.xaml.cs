using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommonServiceLocator;
using MicroMonitor.Config;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Services.EventLog;
using MicroMonitor.Services.MicroLog;
using MicroMonitor.Utilities;
using MicroMonitor.Views.DetailsView;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;

namespace MicroMonitor.Views.MainView
{
    public partial class MainWindow : Window
    {
        private readonly EventLogReader _eventLogReader = new EventLogReader();
        private readonly EventLogPoller _eventLogPoller = new EventLogPoller();
        private readonly MicroLogReader _microLogReader = new MicroLogReader();
        private readonly Timer _nextReadTimer = new Timer();
        private DateTime _lastReadTime = DateTime.MinValue;
        private DateTime _expectedNextReadTime = DateTime.MinValue;
        private string _peekWindowId;
        private Window _peekWindow;
        private readonly List<Window> _openDetailWindows = new List<Window>();
        private bool _activatedOnce = false;
        private IEnumerable<MicroLogEntry> _logEntries = new List<MicroLogEntry>();

        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            
            var store = ServiceLocator.Current.GetInstance<IAppStore>();
            _viewModel = new MainWindowViewModel(this, store);
            DataContext = _viewModel.State;
            
            Loaded += async (sender, args) => await _viewModel.OnLoaded(sender, args);
            KeyDown += async (sender, args) => await _viewModel.OnKeyDown(sender, args);
            KeyUp += async (sender, args) => await _viewModel.OnKeyUp(sender, args);
            Activated += async (sender, args) => await _viewModel.OnActivated(sender, args);
        }
        
        private async Task Refresh()
        {
            ShowOverlay();

            await GetAndBindEvents(AppConfiguration.LogName());

            HideOverlay();
        }

        private void OpenPeekWindow(MicroLogEntry logEntry, bool fullscreen = false)
        {
            if (_peekWindow != null && _peekWindowId != logEntry.Id)
            {
                _peekWindow?.Close();

                ShowPeekWindow(logEntry, fullscreen);
                Focus();
            }

            if (_peekWindow == null)
            {
                ShowPeekWindow(logEntry, fullscreen);
                Focus();
            }
        }

        private void ShowPeekWindow(MicroLogEntry logEntry, bool fullscreen = false)
        {
            _peekWindow = CreateDetailsWindow(logEntry);
            _peekWindow.Show();

            if (fullscreen)
            {
                _peekWindow.WindowState = WindowState.Maximized;
            }

            _peekWindowId = logEntry.Id;
        }
       
        private async Task GetAndBindEvents(string logName)
        {
            var logEntries = await _eventLogReader.ReadEventLogAsync(logName);

            GroupAndBindLogEntries(logEntries.ToArray());
            
            UpdateLastRead();
        }

        private void ShowOverlay()
        {
            Overlay.Visibility = Visibility.Visible;
        }

        private void HideOverlay()
        {
            Overlay.Visibility = Visibility.Collapsed;
        }
        
        private void GroupAndBindLogEntries(MicroLogEntry[] logEntries)
        {
            _logEntries = logEntries;

            var grouped = logEntries.GroupBy(e => e.Timestamp.Date).Select(grp => new
            {
                Key = grp.Key.Date == DateTime.Today ? "Today" : grp.Key.ToString("dd.MM.yy"),
                LogEntries = grp.Select(e => e)
            });

            LogEntries.ItemsSource = grouped;
        }

        private void UpdateLastRead()
        {
            LastRead.Text = $"Last read: {DateTime.Now:HH:mm:ss}";
        }

        private void OnShowLogEntryDetails(object sender, RoutedEventArgs e)
        {
            _viewModel.OnShowLogEntryDetails(sender, e);
        }
        
        private Window CreateDetailsWindow(MicroLogEntry logEntry)
        {
            var configuredHeight = AppConfiguration.DetailsWindowHeight();
            var height = configuredHeight > 0 ? configuredHeight : Height;

            var top = AppConfiguration.DetailsWindowGrowDirection() == GrowDirection.Down
                ? Top
                : Top + (Height - height);

            const int marginBuffer = 20;

            var detailsWindow = new LogEntryDetailsWindow
            {
                LogEntry = logEntry,
                Left = Left + Width + marginBuffer,
                Top = top,
                Height = height,
                Width = AppConfiguration.DetailsWindowWidth()
            };
            
            return detailsWindow;
        }

        private async void OnReadNow(object sender, RoutedEventArgs e)
        {
            await _viewModel.OnReadNow(sender, e);
        }
        
        private async void OnCloseAllDetailWindows(object sender, RoutedEventArgs e)
        {
            await _viewModel.OnCloseAllDetailWindows(sender, e);
        }

        private void OnLogEntryClick(object sender, MouseButtonEventArgs e)
        {
            var stackPanel = (StackPanel) sender;
            var logEntry = (MicroLogEntry) stackPanel.DataContext;

            if (e.ChangedButton == MouseButton.Left)
            {
                OpenPeekWindow(logEntry);
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                OpenPeekWindow(logEntry, true);
            }
        }

        private Border _currentMouseOverBorder;
        
        private void OnMouseOverLogEntry(object sender, MouseEventArgs e)
        {
            var border = (Border)sender;
            _currentMouseOverBorder = border;

            var brush = (SolidColorBrush)border.Background;
            border.Background = new SolidColorBrush(brush.Color.ChangeLightness(3));

            // Store original brush in Tag so it can be restored later
            border.Tag = brush;
        }

        private void OnMouseLeaveLogEntry(object sender, MouseEventArgs e)
        {
            var border = (Border)sender;

            if (_currentMouseOverBorder != null && Equals(border, _currentMouseOverBorder))
            {
                // Restore original brush
                var brush = (SolidColorBrush)border.Tag;
                border.Background = brush;
            }
        }
    }
}
