using System.Windows;
using MediatR;
using MicroMonitor.Views.MainView;

namespace MicroMonitor.Actions
{
    class ToggleHeaderPanelVisibility : INotification
    {
        public Visibility Visibility { get; }

        public ToggleHeaderPanelVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }
    }

    class ToggleOverlayVisibility : INotification
    {
        public Visibility Visibility { get; }

        public ToggleOverlayVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }
    }

    class MainWindowActivated : INotification { }

    class SetDefaultWindowWidthAndHeight : INotification { }

    class WindowPositionChanged : INotification
    {
        public double Left { get; }
        public double Top { get; }

        public WindowPositionChanged(double left, double top)
        {
            Left = left;
            Top = top;
        }
    }

    class WindowSizeChanged : INotification
    {
        public int Width { get; }
        public int Height { get; }

        public WindowSizeChanged(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    class SetDefaultWindowPosition : INotification { }

    class SetMainWindow : INotification
    {
        public MainWindow MainWindow { get; }

        public SetMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }
    }

    class SetNextReadText : INotification
    {
        public string NextReadText { get; }

        public SetNextReadText(string nextReadText)
        {
            NextReadText = nextReadText;
        }
    }

    class SetLastReadText : INotification
    {
        public string LastReadText { get; }

        public SetLastReadText(string lastReadText)
        {
            LastReadText = lastReadText;
        }
    }
}
