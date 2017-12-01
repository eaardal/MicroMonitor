using System.Windows;
using System.Windows.Controls;
using MediatR;
using MicroMonitor.Views.MainView;

namespace MicroMonitor.Actions
{
    public class ToggleHeaderPanelVisibility : INotification
    {
        public Visibility Visibility { get; }

        public ToggleHeaderPanelVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }
    }

    public class ToggleOverlayVisibility : INotification
    {
        public Visibility Visibility { get; }

        public ToggleOverlayVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }
    }

    public class MainWindowActivated : INotification { }

    public class SetDefaultWindowWidthAndHeight : INotification { }

    public class WindowPositionChanged : INotification
    {
        public double Left { get; }
        public double Top { get; }

        public WindowPositionChanged(double left, double top)
        {
            Left = left;
            Top = top;
        }
    }

    public class WindowSizeChanged : INotification
    {
        public int Width { get; }
        public int Height { get; }

        public WindowSizeChanged(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public class SetDefaultWindowPosition : INotification { }

    public class SetMainWindow : INotification
    {
        public MainWindow MainWindow { get; }

        public SetMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }
    }

    public class SetNextReadText : INotification
    {
        public string NextReadText { get; }

        public SetNextReadText(string nextReadText)
        {
            NextReadText = nextReadText;
        }
    }

    public class SetLastReadText : INotification
    {
        public string LastReadText { get; }

        public SetLastReadText(string lastReadText)
        {
            LastReadText = lastReadText;
        }
    }

    public class MouseEnterLogEntryBoundaries : INotification
    {
        public Border Border { get; }

        public MouseEnterLogEntryBoundaries(Border border)
        {
            Border = border;
        }
    }

    public class MouseLeaveLogEntryBoundaries : INotification
    {
        public Border Border { get; }

        public MouseLeaveLogEntryBoundaries(Border border)
        {
            Border = border;
        }
    }
}
