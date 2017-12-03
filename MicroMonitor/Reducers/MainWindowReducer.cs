using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Utilities;

namespace MicroMonitor.Reducers
{
    public class MainWindowReducer : IMainWindowReducer,
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

        public MainWindowReducer(IAppStore store)
        {
            _state = store.GetState().MainWindowState;
        }

        public void Handle(RefreshEventLogEntriesStart message)
        {
            _state.OverlayVisibility = Visibility.Visible;
        }

        public void Handle(RefreshEventLogEntriesSuccess message)
        {
            _state.OverlayVisibility = Visibility.Collapsed;
            _state.LastReadText = $"Last read: {DateTime.Now:HH:mm:ss}";

            UpdateEventLogEntries(message.EventLogEntries);
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
            UpdateEventLogEntries(message.EventLogEntries);
        }

        private void UpdateEventLogEntries(IEnumerable<MicroLogEntry> logEntries)
        {
            var existingLogEntries = _state.LogEntries.Select(e => e.Id);

            var newLogEntries = logEntries
                .Where(entry => !existingLogEntries.Contains(entry.Id))
                .OrderBy(entry => entry.Timestamp)
                .ToImmutableList();

            foreach (var logEntry in newLogEntries)
            {
                _state.LogEntries.Insert(0, logEntry);
            }

            var groupedLogEntries = LogEntryUtils.GroupLogEntriesByDate(newLogEntries);
            var existingGroups = _state.GroupedLogEntries.Select(grp => grp.Key).ToArray();

            foreach (var grouping in groupedLogEntries)
            {
                if (existingGroups.Contains(grouping.Key))
                {
                    var grp = _state.GroupedLogEntries.Single(g => g.Key == grouping.Key);

                    foreach (var entry in grouping.LogEntries.OrderBy(e => e.Timestamp))
                    {
                        grp.LogEntries.Insert(0, entry);
                    }
                }
                else
                {
                    _state.GroupedLogEntries.Insert(0, grouping);
                }
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
