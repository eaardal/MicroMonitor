using System.Windows;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    class PeekWindowState : ObservableObject
    {
        private string _peekWindowId;
        private Window _peekWindow;

        public string PeekWindowId
        {
            get => _peekWindowId;
            set => SetProperty(ref _peekWindowId, value);
        }

        public Window PeekWindow
        {
            get => _peekWindow;
            set => SetProperty(ref _peekWindow, value);
        }
    }
}