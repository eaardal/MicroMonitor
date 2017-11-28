using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;
using MicroMonitor.Utilities;

namespace MicroMonitor.Reducers
{
    class MainWindowReducer : IReducer,
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
        INotificationHandler<UpdateEventLogEntries>,
        INotificationHandler<CloseAllOpenDetailsWindows>,
        INotificationHandler<MouseEnterLogEntryBoundaries>,
        INotificationHandler<MouseLeaveLogEntryBoundaries>
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

        public void Handle(SetMainWindow message)
        {
            _state.Window = message.MainWindow;
        }

        public void Handle(SetLastReadText message)
        {
            _state.LastReadText = message.LastReadText;
        }

        public void Handle(UpdateEventLogEntries message)
        {
            _state.LogEntries.Clear();

            foreach (var logEntry in message.EventLogEntries)
            {
                _state.LogEntries.Add(logEntry);
            }

            _state.GroupedLogEntries.Clear();

            var groupedLogEntries = LogEntryUtils.GroupLogEntriesByDate(_state.LogEntries);

            foreach (var logEntry in groupedLogEntries)
            {
                _state.GroupedLogEntries.Add(logEntry);
            }
        }

        public void Handle(CloseAllOpenDetailsWindows message)
        {
            _state.IsCloseAllDetailWindowsButtonEnabled = false;
        }

        public void Handle(MouseEnterLogEntryBoundaries message)
        {
            _state.CurrentMouseOverBorder = message.Border;

            var brush = (SolidColorBrush)message.Border.Background;
            _state.CurrentMouseOverBorder.Background = new SolidColorBrush(brush.Color.ChangeLightness(3));

            // Store original brush in Tag so it can be restored later
            _state.CurrentMouseOverBorder.Tag = brush;
        }

        public void Handle(MouseLeaveLogEntryBoundaries message)
        {
            var currentMouseOverBorder = _state.CurrentMouseOverBorder;

            if (currentMouseOverBorder != null && Equals(message.Border, currentMouseOverBorder))
            {
                // Restore original brush
                var brush = (SolidColorBrush)currentMouseOverBorder.Tag;
                currentMouseOverBorder.Background = brush;
            }
        }
    }
}
