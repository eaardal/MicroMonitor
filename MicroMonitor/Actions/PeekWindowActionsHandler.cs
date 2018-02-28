using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using MicroMonitor.Config;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Views.DetailsView;

namespace MicroMonitor.Actions
{
    internal class PeekWindowActionsHandler : 
        INotificationHandler<OpenPeekWindowUnderMouseCursor>, 
        INotificationHandler<OpenPeekWindowForNumericKey>, 
        INotificationHandler<TraverseDownAndOpenPeekWindow>,
        INotificationHandler<TraverseUpAndOpenPeekWindow>
    {
        private readonly IAppStore _store;

        public PeekWindowActionsHandler(IAppStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task Handle(OpenPeekWindowUnderMouseCursor message, CancellationToken cancellationToken)
        {
            var ele = Mouse.DirectlyOver as UIElement;

            var textBlock = ele as TextBlock;

            if (textBlock != null)
            {
                var logEntry = textBlock.DataContext as MicroLogEntry;

                if (logEntry == null)
                {
                    Logger.Debug($"Could not cast TextBlock.DataContext to {typeof(MicroLogEntry).FullName}");
                    return;
                }
                
                await OpenPeekWindow(logEntry, message.OpenFullscreen);
            }
        }

        private async Task OpenPeekWindow(MicroLogEntry logEntry, bool openFullscreen)
        {
            var state = _store.GetState();

            var mainWindow = state.MainWindowState.Window;

            var peekWindow = state.PeekWindowState.PeekWindow;
            var peekWindowId = state.PeekWindowState.PeekWindowId;

            if (peekWindow != null && peekWindowId != logEntry.Id)
            {
                peekWindow?.Close();
            }

            if (peekWindow == null)
            {
                ShowPeekWindow(mainWindow, logEntry, openFullscreen);
            }

            (var newPeekWindow, var newPeekWindowId) = ShowPeekWindow(mainWindow, logEntry, openFullscreen);

            await _store.Dispatch(new OpenedNewPeekWindow(newPeekWindow, newPeekWindowId));

            mainWindow.Focus();
        }

        private static (Window newPeekWindow, string newPeekWindowId) ShowPeekWindow(Window parent, MicroLogEntry logEntry, bool fullscreen = false)
        {
            var newPeekWindow = CreateDetailsWindow(parent, logEntry);
            newPeekWindow.Show();

            if (fullscreen)
            {
                newPeekWindow.WindowState = WindowState.Maximized;
            }

            return (newPeekWindow, logEntry.Id);
        }

        public async Task Handle(OpenPeekWindowForNumericKey message, CancellationToken cancellationToken)
        {
            var state = _store.GetState();
            var logEntries = state.MainWindowState.LogEntries;

            var key = message.KeyEventArgs.Key.ToString().Last();
            var keyNum = int.Parse(key.ToString());

            if (keyNum > logEntries.Count)
            {
                return;
            }

            var logEntry = logEntries.ElementAt(keyNum - 1);

            if (KeyboardFacade.IsLeftCtrlDown())
            {
                var newDetailsWindow = OpenNewDetailsWindow(logEntry, true);

                await _store.Dispatch(new CreatedNewDetailsWindow(newDetailsWindow));
            }
            else
            {
                await OpenPeekWindow(logEntry, KeyboardFacade.IsLeftShiftDown());
            }
        }

        private Window OpenNewDetailsWindow(MicroLogEntry logEntry, bool keepFocus = false)
        {
            var mainWindow = _store.GetState().MainWindowState.Window;

            var detailsWindow = CreateDetailsWindow(mainWindow, logEntry);
            detailsWindow.Show();
            
            if (keepFocus)
            {
                mainWindow.Focus();
            }

            return detailsWindow;
        }

        private static Window CreateDetailsWindow(Window parent, MicroLogEntry logEntry)
        {
            var configuredHeight = AppConfiguration.DetailsWindowHeight();
            var height = configuredHeight > 0 ? configuredHeight : parent.Height;

            var top = AppConfiguration.DetailsWindowGrowDirection() == GrowDirection.Down
                ? parent.Top
                : parent.Top + (parent.Height - height);

            const int marginBuffer = 20;

            var detailsWindow = new LogEntryDetailsWindow
            {
                LogEntry = logEntry,
                Left = parent.Left + parent.Width + marginBuffer,
                Top = top,
                Height = height,
                Width = AppConfiguration.DetailsWindowWidth()
            };

            return detailsWindow;
        }

        public async Task Handle(TraverseDownAndOpenPeekWindow message, CancellationToken cancellationToken)
        {
            var state = _store.GetState();
            var logEntries = state.MainWindowState.LogEntries;
            var traversingIndex = state.MainWindowState.TraversingIndex;

            if (logEntries.Count >= traversingIndex + 1)
            {
                var newTraversingIndex = traversingIndex + 1;

                await _store.Dispatch(new SetTraversingIndex(newTraversingIndex));

                var logEntry = logEntries.ElementAt(newTraversingIndex);

                if (KeyboardFacade.IsLeftCtrlDown())
                {
                    OpenNewDetailsWindow(logEntry, true);
                }
                else
                {
                    await OpenPeekWindow(logEntry, KeyboardFacade.IsLeftShiftDown());
                }
            }
        }

        public async Task Handle(TraverseUpAndOpenPeekWindow notification, CancellationToken cancellationToken)
        {
            var state = _store.GetState();
            var logEntries = state.MainWindowState.LogEntries;
            var traversingIndex = state.MainWindowState.TraversingIndex;

            if (traversingIndex - 1 >= 0)
            {
                var newTraversingIndex = traversingIndex - 1;

                await _store.Dispatch(new SetTraversingIndex(newTraversingIndex));

                var logEntry = logEntries.ElementAt(newTraversingIndex);

                if (KeyboardFacade.IsLeftCtrlDown())
                {
                    OpenNewDetailsWindow(logEntry, true);
                }
                else
                {
                    await OpenPeekWindow(logEntry, KeyboardFacade.IsLeftShiftDown());
                }
            }
        }
    }
}
