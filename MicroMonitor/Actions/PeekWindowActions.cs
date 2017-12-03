using System.Windows;
using System.Windows.Input;
using MediatR;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    public class OpenPeekWindowUnderMouseCursor : Action
    {
        public KeyEventArgs KeyEventArgs { get; }
        public bool OpenFullscreen { get; }

        public OpenPeekWindowUnderMouseCursor(KeyEventArgs keyEventArgs, bool openFullscreen = false)
        {
            KeyEventArgs = keyEventArgs;
            OpenFullscreen = openFullscreen;
        }

        public override string ToString()
        {
            return "OPEN_PEEK_WINDOW_UNDER_MOUSE_CURSOR";
        }
    }

    public class OpenedNewPeekWindow : Action
    {
        public Window NewPeekWindow { get; }
        public string NewPeekWindowId { get; }

        public OpenedNewPeekWindow(Window newPeekWindow, string newPeekWindowId)
        {
            NewPeekWindow = newPeekWindow;
            NewPeekWindowId = newPeekWindowId;
        }

        public override string ToString()
        {
            return "OPENED_NEW_PEEK_WINDOW";
        }
    }

    public class OpenPeekWindow : Action
    {
        public MicroLogEntry LogEntry { get; }
        public bool OpenFullscreen { get; }

        public OpenPeekWindow(MicroLogEntry logEntry, bool openFullscreen = false)
        {
            LogEntry = logEntry;
            OpenFullscreen = openFullscreen;
        }

        public override string ToString()
        {
            return "OPEN_PEEK_WINDOW";
        }
    }

    public class OpenPeekWindowForNumericKey : Action
    {
        public KeyEventArgs KeyEventArgs { get; }
        public bool OpenFullscreen { get; }

        public OpenPeekWindowForNumericKey(KeyEventArgs keyEventArgs, bool openFullscreen = false)
        {
            KeyEventArgs = keyEventArgs;
            OpenFullscreen = openFullscreen;
        }

        public override string ToString()
        {
            return "OPEN_PEEK_WINDOW_FOR_NUMERIC_KEY";
        }
    }

    public class CreatedNewDetailsWindow : Action
    {
        public Window NewDetailsWindow { get; }

        public CreatedNewDetailsWindow(Window newDetailsWindow)
        {
            NewDetailsWindow = newDetailsWindow;
        }

        public override string ToString()
        {
            return "CREATED_NEW_DETAILS_WINDOW";
        }
    }

    public class TraverseDownAndOpenPeekWindow : Action
    {
        public override string ToString()
        {
            return "TRAVERSE_DOWN_AND_OPEN_PEEK_WINDOW";
        }
    }

    public class TraverseUpAndOpenPeekWindow : Action
    {
        public override string ToString()
        {
            return "TRAVERSE_UP_AND_OPEN_PEEK_WINDOW";
        }
    }

    public class SetTraversingIndex : Action
    {
        public int TraversingIndex { get; }

        public SetTraversingIndex(int traversingIndex)
        {
            TraversingIndex = traversingIndex;
        }

        public override string ToString()
        {
            return "SET_TRAVERSING_INDEX";
        }
    }
}
