using System;
using System.Linq;
using System.Windows;
using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Utilities;
using MicroMonitor.Views.MainView;

namespace MicroMonitor.Reducers
{
    class MainWindowReducer : 
        INotificationHandler<RefreshEventLogEntriesStart>,
        INotificationHandler<RefreshEventLogEntriesSuccess>,
        INotificationHandler<RefreshEventLogEntriesError>,
        INotificationHandler<ToggleHeaderPanelVisibility>,
        INotificationHandler<CreatedNewDetailsWindow>,
        INotificationHandler<SetTraversingIndex>,
        INotificationHandler<MainWindowActivated>,
        INotificationHandler<WindowPositionChanged>,
        INotificationHandler<WindowSizeChanged>,
        INotificationHandler<SetMainWindow>,
        INotificationHandler<SetLastReadText>,
        INotificationHandler<UpdateEventLogEntries>
    {
        private readonly MainWindowState _state;

        public MainWindowReducer()
        {
            _state = new MainWindowState();
        }

        public void Handle(RefreshEventLogEntriesStart message)
        {
            _state.OverlayVisibility = Visibility.Visible;
        }

        public void Handle(RefreshEventLogEntriesSuccess message)
        {
            _state.OverlayVisibility = Visibility.Collapsed;
            _state.LastReadText = $"Last read: {DateTime.Now:HH:mm:ss}";

            foreach (var logEntry in message.EventLogEntries)
            {
                _state.GroupedLogEntries.Add(logEntry);
            }
        }

        public void Handle(RefreshEventLogEntriesError message)
        {
            _state.OverlayVisibility = Visibility.Collapsed;
        }

        public void Handle(ToggleHeaderPanelVisibility message)
        {
            _state.HeaderPanelVisibility = message.Visibility;
        }

        public void Handle(CreatedNewDetailsWindow message)
        {
            _state.IsCloseAllDetailWindowsButtonEnabled = true;
        }

        public void Handle(SetTraversingIndex message)
        {
            _state.TraversingIndex = message.TraversingIndex;
        }

        public void Handle(MainWindowActivated message)
        {
            _state.IsActivatedOnce = true;
        }

        public void Handle(WindowPositionChanged message)
        {
            _state.WindowLeft = message.Left;
            _state.WindowTop = message.Top;
        }

        public void Handle(WindowSizeChanged message)
        {
            _state.WindowHeight = message.Height;
            _state.WindowWidth = message.Width;
        }

        public void Handle(SetMainWindow notification)
        {
            _state.Window = notification.MainWindow;
        }

        public void Handle(SetLastReadText notification)
        {
            _state.LastReadText = notification.LastReadText;
        }

        public void Handle(UpdateEventLogEntries notification)
        {
            _state.LogEntries = notification.EventLogEntries.ToList();

            _state.GroupedLogEntries.Clear();

            var groupedLogEntries = LogEntryUtils.GroupLogEntriesByDate(_state.LogEntries);

            foreach (var logEntry in groupedLogEntries)
            {
                _state.GroupedLogEntries.Add(logEntry);
            }
        }
    }
}
