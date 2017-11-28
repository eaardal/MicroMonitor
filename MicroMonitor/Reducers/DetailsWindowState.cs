using System.Collections.ObjectModel;
using System.Windows;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    class DetailsWindowState : ObservableObject
    {
        private ObservableCollection<Window> _openDetailsWindows = new ObservableCollection<Window>();

        public ObservableCollection<Window> OpenDetailsWindows
        {
            get => _openDetailsWindows;
            set => SetProperty(ref _openDetailsWindows, value);
        }
    }
}
