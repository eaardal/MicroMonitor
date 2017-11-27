using MicroMonitor.Reducers;
using MicroMonitor.Views.MainView;

namespace MicroMonitor
{
    class AppState
    {
        public MainWindowModel MainWindowState { get; set; }
        public PeekWindowState PeekWindowState { get; set; }
    }
}
