using System.Collections.ObjectModel;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Model
{
    public class GroupedMicroLogEntry : ObservableObject
    {
        private string _key;

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public ObservableCollection<MicroLogEntry> LogEntries { get; } = new ObservableCollection<MicroLogEntry>();
    }
}
