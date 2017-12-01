using MediatR;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    public class OpenNewDetailsWindow : INotification
    {
        public MicroLogEntry LogEntry { get; }

        public OpenNewDetailsWindow(MicroLogEntry logEntry)
        {
            LogEntry = logEntry;
        }
    }

    public class CloseAllOpenDetailsWindows : INotification
    {
        
    }
}
