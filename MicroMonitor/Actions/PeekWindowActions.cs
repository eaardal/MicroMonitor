using System.Windows;
using System.Windows.Input;
using MediatR;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    public class OpenPeekWindowUnderMouseCursor : INotification
    {
        public KeyEventArgs KeyEventArgs { get; }
        public bool OpenFullscreen { get; }

        public OpenPeekWindowUnderMouseCursor(KeyEventArgs keyEventArgs, bool openFullscreen = false)
        {
            KeyEventArgs = keyEventArgs;
            OpenFullscreen = openFullscreen;
        }
    }

    public class OpenedNewPeekWindow : INotification
    {
        public Window NewPeekWindow { get; }
        public string NewPeekWindowId { get; }

        public OpenedNewPeekWindow(Window newPeekWindow, string newPeekWindowId)
        {
            NewPeekWindow = newPeekWindow;
            NewPeekWindowId = newPeekWindowId;
        }
    }

    public class OpenPeekWindow : INotification
    {
        public MicroLogEntry LogEntry { get; }
        public bool OpenFullscreen { get; }

        public OpenPeekWindow(MicroLogEntry logEntry, bool openFullscreen = false)
        {
            LogEntry = logEntry;
            OpenFullscreen = openFullscreen;
        }
    }

    public class OpenPeekWindowForNumericKey : INotification
    {
        public KeyEventArgs KeyEventArgs { get; }
        public bool OpenFullscreen { get; }

        public OpenPeekWindowForNumericKey(KeyEventArgs keyEventArgs, bool openFullscreen = false)
        {
            KeyEventArgs = keyEventArgs;
            OpenFullscreen = openFullscreen;
        }
    }

    public class CreatedNewDetailsWindow : INotification
    {
        public Window NewDetailsWindow { get; }

        public CreatedNewDetailsWindow(Window newDetailsWindow)
        {
            NewDetailsWindow = newDetailsWindow;
        }
    }

    public class TraverseDownAndOpenPeekWindow : INotification { }

    public class TraverseUpAndOpenPeekWindow : INotification { }

    public class SetTraversingIndex : INotification
    {
        public int TraversingIndex { get; }

        public SetTraversingIndex(int traversingIndex)
        {
            TraversingIndex = traversingIndex;
        }
    }
}
