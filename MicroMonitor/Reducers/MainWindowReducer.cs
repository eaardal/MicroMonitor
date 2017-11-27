using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Views.MainView;

namespace MicroMonitor.Reducers
{
    class MainWindowReducer : IRequestHandler<RefreshEventLogEntriesStart>,
        IRequestHandler<RefreshEventLogEntriesSuccess>,
        IRequestHandler<RefreshEventLogEntriesError>,
        IRequestHandler<ToggleHeaderPanelVisibility>
    {
        private readonly MainWindowModel _state;

        public MainWindowReducer()
        {
            _state = new MainWindowModel();
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
    }
}
