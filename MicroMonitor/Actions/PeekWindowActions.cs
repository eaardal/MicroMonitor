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
    
    class OpenedNewPeekWindow : INotification
    {
        public Window NewPeekWindow { get; }
        public string NewPeekWindowId { get; }

        public OpenedNewPeekWindow(Window newPeekWindow, string newPeekWindowId)
        {
            NewPeekWindow = newPeekWindow;
            NewPeekWindowId = newPeekWindowId;
        }
    }

    class OpenPeekWindowForNumericKey : INotification
    {
        public KeyEventArgs KeyEventArgs { get; }
        public bool OpenFullscreen { get; }

        public OpenPeekWindowForNumericKey(KeyEventArgs keyEventArgs, bool openFullscreen = false)
        {
            KeyEventArgs = keyEventArgs;
            OpenFullscreen = openFullscreen;
        }
    }

    class CreatedNewDetailsWindow : INotification
    {
        public Window NewDetailsWindow { get; }

        public CreatedNewDetailsWindow(Window newDetailsWindow)
        {
            NewDetailsWindow = newDetailsWindow;
        }
    }

    class TraverseDownAndOpenPeekWindow : INotification { }

    class TraverseUpAndOpenPeekWindow : INotification { }

    class SetTraversingIndex : INotification
    {
        public int TraversingIndex { get; }

        public SetTraversingIndex(int traversingIndex)
        {
            TraversingIndex = traversingIndex;
        }
    }
}
