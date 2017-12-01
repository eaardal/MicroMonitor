using MicroMonitor.Reducers;

namespace MicroMonitor
{
    public class AppState
    {
        public MainWindowState MainWindowState { get; } = new MainWindowState();
        public PeekWindowState PeekWindowState { get; } = new PeekWindowState();
        public DetailsWindowState DetailsWindowState { get; } = new DetailsWindowState();
    }
}
