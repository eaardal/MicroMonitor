using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            _state = store.State.MainWindowState;
            //_state.GroupedLogEntries.CollectionChanged += (sender, args) =>
            //{
            //    Logger.Debug("GroupedLogEntries: {@ChangeType}", args.Action);
            //};
        }

        public Task Handle(RefreshEventLogEntriesStart message, CancellationToken cancellationToken)
        {
            _state.OverlayVisibility = Visibility.Visible;

            return Task.CompletedTask;
        }

        public Task Handle(RefreshEventLogEntriesSuccess message, CancellationToken cancellationToken)
        {
            _state.OverlayVisibility = Visibility.Collapsed;
            _state.LastReadText = $"Last read: {DateTime.Now:HH:mm:ss}";

            UpdateEventLogEntries(message.EventLogEntries);

            return Task.CompletedTask;
        }

        public Task Handle(RefreshEventLogEntriesError message, CancellationToken cancellationToken)
        {
            _state.OverlayVisibility = Visibility.Collapsed;

            return Task.CompletedTask;
        }

        public Task Handle(ToggleHeaderPanelVisibility message, CancellationToken cancellationToken)
        {
            _state.HeaderPanelVisibility = message.Visibility;
            
            return Task.CompletedTask;
        }

        public Task Handle(CreatedNewDetailsWindow message, CancellationToken cancellationToken)
        {
            _state.IsCloseAllDetailWindowsButtonEnabled = true;
            
            return Task.CompletedTask;
        }

        public Task Handle(SetTraversingIndex message, CancellationToken cancellationToken)
        {
            _state.TraversingIndex = message.TraversingIndex;
            
            return Task.CompletedTask;
        }

        public Task Handle(MainWindowActivated message, CancellationToken cancellationToken)
        {
            _state.IsActivatedOnce = true;
            
            return Task.CompletedTask;
        }

        public Task Handle(WindowPositionChanged message, CancellationToken cancellationToken)
        {
            _state.WindowLeft = message.Left;
            _state.WindowTop = message.Top;
            
            return Task.CompletedTask;
        }

        public Task Handle(WindowSizeChanged message, CancellationToken cancellationToken)
        {
            _state.WindowHeight = message.Height;
            _state.WindowWidth = message.Width;
            
            return Task.CompletedTask;
        }

        public Task Handle(SetMainWindow message, CancellationToken cancellationToken)
        {
            _state.Window = message.MainWindow;
            
            return Task.CompletedTask;
        }

        public Task Handle(SetLastReadText message, CancellationToken cancellationToken)
        {
            _state.LastReadText = message.LastReadText;
            
            return Task.CompletedTask;
        }

        public Task Handle(UpdateEventLogEntries message, CancellationToken cancellationToken)
        {
            UpdateEventLogEntries(message.EventLogEntries);
            
            return Task.CompletedTask;
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
                //_state.LogEntries.Insert(0, logEntry);
                _state.LogEntries.Add(logEntry);
                //_state.LogEntries.Move(_state.LogEntries.Count, 0);
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
                        //grp.LogEntries.Insert(0, entry);
                        grp.LogEntries.Add(entry);
                        //grp.LogEntries.Move(grp.LogEntries.Count, 0);
                    }
                }
                else
                {
                    //_state.GroupedLogEntries.Insert(0, grouping);
                    _state.GroupedLogEntries.Add(grouping);
                    //_state.GroupedLogEntries.Move(_state.GroupedLogEntries.Count, 0);
                }
            }
        }

        public Task Handle(CloseAllOpenDetailsWindows message, CancellationToken cancellationToken)
        {
            _state.IsCloseAllDetailWindowsButtonEnabled = false;
            
            return Task.CompletedTask;
        }

        public Task Handle(MouseEnterLogEntryBoundaries message, CancellationToken cancellationToken)
        {
            _state.CurrentMouseOverBorder = message.Border;

            var brush = (SolidColorBrush)message.Border.Background;
            _state.CurrentMouseOverBorder.Background = new SolidColorBrush(brush.Color.ChangeLightness(3));

            // Store original brush in Tag so it can be restored later
            _state.CurrentMouseOverBorder.Tag = brush;
            
            return Task.CompletedTask;
        }

        public Task Handle(MouseLeaveLogEntryBoundaries message, CancellationToken cancellationToken)
        {
            var currentMouseOverBorder = _state.CurrentMouseOverBorder;

            if (currentMouseOverBorder != null && Equals(message.Border, currentMouseOverBorder))
            {
                // Restore original brush
                var brush = (SolidColorBrush)currentMouseOverBorder.Tag;
                currentMouseOverBorder.Background = brush;
            }

            return Task.CompletedTask;
        }
    }
}
