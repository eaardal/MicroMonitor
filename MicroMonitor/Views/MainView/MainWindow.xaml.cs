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
using MicroMonitor.Config;
using MicroMonitor.Engine.EventLog;
using MicroMonitor.Engine.MicroLog;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
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
            //Logger.Create();

            InitializeComponent();

            _viewModel = new MainWindowViewModel(this);
            DataContext = _viewModel.Model;

            //HeaderPanel.Visibility = Visibility.Collapsed;
            //DisableCloseAllDetailWindowsButton();
            
            //Loaded += OnLoaded;
            Loaded += _viewModel.OnLoaded;
            KeyDown += async (sender, args) => await OnKeyDown(sender, args);
            KeyUp += OnKeyUp;
            Activated += async (sender, args) => await OnActivated(sender, args);
        }

        //private void EnableCloseAllDetailWindowsButton()
        //{
        //    BtnCloseAllDetailWindows.IsEnabled = true;
        //}

        //private void DisableCloseAllDetailWindowsButton()
        //{
        //    BtnCloseAllDetailWindows.IsEnabled = false;
        //}

        //private void OnLoaded(object o, RoutedEventArgs routedEventArgs)
        //{
        //    Width = AppConfiguration.MainWindowWidth();
        //    Height = AppConfiguration.MainWindowHeight();

        //    var spawnMethod = AppConfiguration.MainWindowSpawnMethod();

        //    switch (spawnMethod)
        //    {
        //        case WindowSpawnMethod.Cursor:
        //            WindowHelper.PositionWindowAtMouseCursor(this);
        //            break;
        //        case WindowSpawnMethod.CenterScreen:
        //            WindowHelper.PositionWindowAtCenterScreen(this);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        private async Task OnActivated(object o, EventArgs eventArgs)
        {
            var logName = AppConfiguration.LogName();
            
            if (!_activatedOnce)
            {
                _activatedOnce = true;

                var pollIntervalSeconds = AppConfiguration.PollIntervalSeconds();

                await GetAndBindEvents(logName);

                StartPollingForEventViewerLogs(logName, pollIntervalSeconds);
            }
        }

        private void OnKeyUp(object o, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                HeaderPanel.Visibility = Visibility.Collapsed;
            }
        }

        private async Task OnKeyDown(object o, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                HeaderPanel.Visibility = Visibility.Visible;
            }

            if (e.Key == Key.LeftShift)
            {
                var ele = Mouse.DirectlyOver as UIElement;
                
                var textBlock = ele as TextBlock;

                if (textBlock != null)
                {
                    Logger.Debug("Mouse is over TextBlock");

                    var logEntry = textBlock.DataContext as MicroLogEntry;

                    if (logEntry == null)
                    {
                        Logger.Debug($"Could not cast TextBlock.DataContext to {typeof(MicroLogEntry).FullName}");
                        return;
                    }

                    OpenPeekWindow(logEntry);
                }
                else
                {
                    Logger.Debug("Mouse is not over TextBlock");
                }
            }

            if (e.Key == Key.R || e.Key == Key.R && KeyboardFacade.IsLeftCtrlDown())
            {
                await Refresh();
            }

            if (e.Key >= Key.D1 && e.Key <= Key.D9)
            {
                var key = e.Key.ToString().Last();
                var keyNum = int.Parse(key.ToString());
                
                if (keyNum > _logEntries.Count())
                {
                    return;
                }
                
                var logEntry = _logEntries.ElementAt(keyNum - 1);

                if (KeyboardFacade.IsLeftCtrlDown())
                {
                    OpenNewDetailsWindow(logEntry, true);
                }
                else
                {
                    OpenPeekWindow(logEntry, KeyboardFacade.IsLeftShiftDown());
                }
            }

            if (e.Key == Key.S || e.Key == Key.Down)
            {
                if (_logEntries.Count() >= _traversingIndex + 1)
                {
                    _traversingIndex++;

                    var logEntry = _logEntries.ElementAt(_traversingIndex);

                    if (KeyboardFacade.IsLeftCtrlDown())
                    {
                        OpenNewDetailsWindow(logEntry, true);
                    }
                    else
                    {
                        OpenPeekWindow(logEntry, KeyboardFacade.IsLeftShiftDown());
                    }
                }
            }

            if (e.Key == Key.W || e.Key == Key.Up)
            {
                if (_traversingIndex - 1 >= 0)
                {
                    _traversingIndex--;

                    var logEntry = _logEntries.ElementAt(_traversingIndex);

                    if (KeyboardFacade.IsLeftCtrlDown())
                    {
                        OpenNewDetailsWindow(logEntry, true);
                    }
                    else
                    {
                        OpenPeekWindow(logEntry, KeyboardFacade.IsLeftShiftDown());
                    }
                }
            }
        }

        private int _traversingIndex = -1;

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
        
        private void StartPollingForEventViewerLogs(string logName, int pollIntervalSeconds)
        {
            _eventLogPoller.StartPollingAtIntervals(logName, pollIntervalSeconds);

            var readInterval = pollIntervalSeconds + 2;

            _expectedNextReadTime = DateTime.Now.AddSeconds(readInterval);

            _nextReadTimer.Interval = 1000;
            _nextReadTimer.Elapsed += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_expectedNextReadTime != DateTime.MinValue)
                    {
                        var nextReadInSeconds = _expectedNextReadTime.Subtract(DateTime.Now).Seconds;
                        NextRead.Text = $"Next read: {nextReadInSeconds}s";
                    }
                    else
                    {
                        NextRead.Text = "Next read: TBD";
                    }

                });
            };
            _nextReadTimer.Start();

            _microLogReader.ReadOnInterval(logName, readInterval, newLogEntries =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _lastReadTime = DateTime.Now;
                    _expectedNextReadTime = _lastReadTime.AddSeconds(readInterval);

                    GroupAndBindLogEntries(newLogEntries.ToArray());
                    UpdateLastRead();
                });
            });
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
            var btn = (Button)sender;
            var logEntry = (MicroLogEntry)btn.DataContext;

            OpenNewDetailsWindow(logEntry);
        }

        private void OpenNewDetailsWindow(MicroLogEntry logEntry, bool keepFocus = false)
        {
            var detailsWindow = CreateDetailsWindow(logEntry);
            detailsWindow.Show();

            _openDetailWindows.Add(detailsWindow);

            BtnCloseAllDetailWindows.IsEnabled = _openDetailWindows.Any();

            if (keepFocus)
            {
                Focus();
            }
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
            await Refresh();
        }
        
        private void OnCloseAllDetailWindows(object sender, RoutedEventArgs e)
        {
            foreach (var openDetailWindow in _openDetailWindows)
            {
                openDetailWindow.Close();
            }

            _openDetailWindows.Clear();

            _viewModel.DisableCloseAllDetailWindowsButton();
            //DisableCloseAllDetailWindowsButton();
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
