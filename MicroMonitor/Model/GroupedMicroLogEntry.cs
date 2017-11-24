using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MicroMonitor.Model
{
    class GroupedMicroLogEntry
    {
        public string Key { get; set; }
        public ObservableCollection<MicroLogEntry> LogEntries { get; set; } = new ObservableCollection<MicroLogEntry>();
    }
}
