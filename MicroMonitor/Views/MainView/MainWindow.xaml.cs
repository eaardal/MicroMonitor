using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommonServiceLocator;
using MicroMonitor.Config;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Utilities;
using MicroMonitor.Views.DetailsView;

namespace MicroMonitor.Views.MainView
{
    public partial class MainWindow
    {
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
        
        private void OnShowLogEntryDetails(object sender, RoutedEventArgs e)
        {
            _viewModel.OnShowLogEntryDetails(sender, e);
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
