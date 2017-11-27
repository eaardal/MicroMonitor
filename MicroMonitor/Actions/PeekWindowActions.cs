using System.Windows.Input;
using MediatR;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    public class OpenPeekWindowUnderMouseCursor : IRequest
    {
        public KeyEventArgs KeyEventArgs { get; }

        public OpenPeekWindowUnderMouseCursor(KeyEventArgs keyEventArgs)
        {
            KeyEventArgs = keyEventArgs;
        }
    }

    class ShowPeekWindow
    {
        public MicroLogEntry LogEntry { get; }
        public bool Fullscreen { get; }

        public ShowPeekWindow(MicroLogEntry logEntry, bool fullscreen = false)
        {
            LogEntry = logEntry;
            Fullscreen = fullscreen;
        }
    }
}
