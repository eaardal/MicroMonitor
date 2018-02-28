using System.Collections.ObjectModel;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Model
{
    public class GroupedMicroLogEntry : ObservableObject
    {
        private string _key;
        private ObservableCollection<MicroLogEntry> _logEntries = new ObservableCollection2<MicroLogEntry>();

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public ObservableCollection<MicroLogEntry> LogEntries
        {
            get => _logEntries;
            set => SetProperty(ref _logEntries, value);
        }
    }
}
