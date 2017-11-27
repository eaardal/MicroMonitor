using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MicroMonitor.Actions;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Reducers;

namespace MicroMonitor.Views.MainView
{
    class MainWindowViewModel
    {
        private readonly IAppStore _store;

        public MainWindowState State { get; }

        public MainWindowViewModel(MainWindow window, IAppStore store)
        {
            _store = store;
            _store.Dispatch(new SetMainWindow(window));
            
            State = _store.GetState().MainWindowState;

            Logger.Create();
        }

        public async Task OnKeyUp(object o, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                await _store.Dispatch(new ToggleHeaderPanelVisibility(Visibility.Visible));
            }
        }

        public async Task OnKeyDown(object o, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                await _store.Dispatch(new ToggleHeaderPanelVisibility(Visibility.Collapsed));
            }

            if (e.Key == Key.LeftShift)
            {
                await _store.Dispatch(new OpenPeekWindowUnderMouseCursor(e));
            }

            if (e.Key == Key.R || e.Key == Key.R && KeyboardFacade.IsLeftCtrlDown())
            {
                await _store.Dispatch(new RefreshEventLogEntries());
            }
            
            if (e.Key >= Key.D1 && e.Key <= Key.D9)
            {
                await _store.Dispatch(new OpenPeekWindowForNumericKey(e));
            }

            if (e.Key == Key.S || e.Key == Key.Down)
            {
                await _store.Dispatch(new TraverseDownAndOpenPeekWindow());
            }

            if (e.Key == Key.W || e.Key == Key.Up)
            {
                await _store.Dispatch(new TraverseUpAndOpenPeekWindow());
            }
        }
        
        public async Task OnActivated(object o, EventArgs eventArgs)
        {
            var state = _store.GetState();
            
            if (!state.MainWindowState.IsActivatedOnce)
            {
                await _store.Dispatch(new MainWindowActivated());
                await _store.Dispatch(new RefreshEventLogEntries());
                await _store.Dispatch(new StartPollingForEventLogEntries());
            }
        }
        
        public async Task OnLoaded(object o, RoutedEventArgs routedEventArgs)
        {
            await _store.Dispatch(new SetDefaultWindowWidthAndHeight());

            await _store.Dispatch(new SetDefaultWindowPosition());
        }
        
        public void DisableCloseAllDetailWindowsButton()
        {
            State.IsCloseAllDetailWindowsButtonEnabled = false;
        }

        public void OnShowLogEntryDetails(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var logEntry = (MicroLogEntry)btn.DataContext;

            OpenNewDetailsWindow(logEntry);
        }

        public async Task OnReadNow(object sender, RoutedEventArgs e)
        {
            await _store.Dispatch(new RefreshEventLogEntries());
        }

        public async Task OnCloseAllDetailWindows(object sender, RoutedEventArgs e)
        {
            foreach (var openDetailWindow in _openDetailWindows)
            {
                openDetailWindow.Close();
            }

            _openDetailWindows.Clear();

            _viewModel.DisableCloseAllDetailWindowsButton();
            //DisableCloseAllDetailWindowsButton();
        }
    }
}
