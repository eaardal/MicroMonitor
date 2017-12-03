using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace MicroMonitor.Model
{
    public class GroupedMicroLogEntry
    {
        public string Key { get; set; }
        public ObservableCollection<MicroLogEntry> LogEntries { get; set; } = new ObservableCollection<MicroLogEntry>();
    }
}
