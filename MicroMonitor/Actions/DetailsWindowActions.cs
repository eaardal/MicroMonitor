using MediatR;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    class OpenNewDetailsWindow : INotification
    {
        public MicroLogEntry LogEntry { get; }

        public OpenNewDetailsWindow(MicroLogEntry logEntry)
        {
            LogEntry = logEntry;
        }
    }

    class CloseAllOpenDetailsWindows : INotification
    {
        
    }
}
