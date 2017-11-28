using MicroMonitor.Reducers;
using MicroMonitor.Views.MainView;

namespace MicroMonitor
{
    class AppState
    {
        public MainWindowState MainWindowState { get; } = new MainWindowState();
        public PeekWindowState PeekWindowState { get; } = new PeekWindowState();
        public DetailsWindowState DetailsWindowState { get; } = new DetailsWindowState();
    }
}
