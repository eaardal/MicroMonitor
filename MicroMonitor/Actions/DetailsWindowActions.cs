using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    public class OpenNewDetailsWindow : Action
    {
        public MicroLogEntry LogEntry { get; }

        public OpenNewDetailsWindow(MicroLogEntry logEntry)
        {
            LogEntry = logEntry;
        }

        public override string ToString()
        {
            return "OPEN_NEW_DETAILS_WINDOW";
        }
    }

    public class CloseAllOpenDetailsWindows : Action
    {
        public override string ToString()
        {
            return "CLOSE_ALL_OPEN_DETAILS_WINDOWS";
        }
    }
}
