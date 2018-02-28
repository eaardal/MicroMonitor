using System.Windows;
using System.Windows.Input;
using CommonServiceLocator;
using MicroMonitor.Infrastructure;

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
            DataContext = store.State.MainWindowState;
            
            Loaded += async (sender, args) => await _viewModel.OnLoaded(sender, args);
            KeyDown += async (sender, args) => await _viewModel.OnKeyDown(sender, args);
            KeyUp += async (sender, args) => await _viewModel.OnKeyUp(sender, args);
            Activated += async (sender, args) => await _viewModel.OnActivated(sender, args);
        }
        
        private async void OnShowLogEntryDetails(object sender, RoutedEventArgs e)
        {
            await _viewModel.OnShowLogEntryDetails(sender, e);
        }
        
        private async void OnReadNow(object sender, RoutedEventArgs e)
        {
            await _viewModel.OnReadNow(sender, e);
        }
        
        private async void OnCloseAllDetailWindows(object sender, RoutedEventArgs e)
        {
            await _viewModel.OnCloseAllDetailWindows(sender, e);
        }

        private async void OnLogEntryClick(object sender, MouseButtonEventArgs e)
        {
            await _viewModel.OnLogEntryClick(sender, e);
        }
        
        private async void OnMouseOverLogEntry(object sender, MouseEventArgs e)
        {
            await _viewModel.OnMouseOverLogEntry(sender, e);
        }

        private async void OnMouseLeaveLogEntry(object sender, MouseEventArgs e)
        {
            await _viewModel.OnMouseLeaveLogEntry(sender, e);
        }
    }
}
