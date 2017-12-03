using System.Windows;
using System.Windows.Controls;
using MediatR;
using MicroMonitor.Infrastructure;
using MicroMonitor.Views.MainView;

namespace MicroMonitor.Actions
{
    public class ToggleHeaderPanelVisibility : Action
    {
        public Visibility Visibility { get; }

        public ToggleHeaderPanelVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }

        public override string ToString()
        {
            return "TOGGLE_HEADER_PANEL_VISIBILITY";
        }
    }

    public class ToggleOverlayVisibility : Action
    {
        public Visibility Visibility { get; }

        public ToggleOverlayVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }

        public override string ToString()
        {
            return "TOGGLE_OVERLAY_VISIBILITY";
        }
    }

    public class MainWindowActivated : Action
    {
        public override string ToString()
        {
            return "MAIN_WINDOW_ACTIVATED";
        }
    }

    public class SetDefaultWindowWidthAndHeight : Action
    {
        public override string ToString()
        {
            return "SET_DEFAULT_WINDOW_WIDTH_AND_HEIGHT";
        }
    }

    public class WindowPositionChanged : Action
    {
        public double Left { get; }
        public double Top { get; }

        public WindowPositionChanged(double left, double top)
        {
            Left = left;
            Top = top;
        }

        public override string ToString()
        {
            return "WINDOW_POSITION_CHANGED";
        }
    }

    public class WindowSizeChanged : Action
    {
        public int Width { get; }
        public int Height { get; }

        public WindowSizeChanged(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return "WINDOW_SIZE_CHANGED";
        }
    }

    public class SetDefaultWindowPosition : Action
    {
        public override string ToString()
        {
            return "SET_DEFAULT_WINDOW_POSITION";
        }
    }

    public class SetMainWindow : Action
    {
        public MainWindow MainWindow { get; }

        public SetMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public override string ToString()
        {
            return "SET_MAIN_WINDOW";
        }
    }

    public class SetNextReadText : Action
    {
        public string NextReadText { get; }

        public SetNextReadText(string nextReadText)
        {
            NextReadText = nextReadText;
        }

        public override string ToString()
        {
            return "SET_NEXT_READ_TIME";
        }
    }

    public class SetLastReadText : Action
    {
        public string LastReadText { get; }

        public SetLastReadText(string lastReadText)
        {
            LastReadText = lastReadText;
        }

        public override string ToString()
        {
            return "SET_LAST_READ_TIME";
        }
    }

    public class MouseEnterLogEntryBoundaries : Action
    {
        public Border Border { get; }

        public MouseEnterLogEntryBoundaries(Border border)
        {
            Border = border;
        }

        public override string ToString()
        {
            return "MOUSE_ENTER_LOG_ENTRY_BOUNDARIES";
        }
    }

    public class MouseLeaveLogEntryBoundaries : Action
    {
        public Border Border { get; }

        public MouseLeaveLogEntryBoundaries(Border border)
        {
            Border = border;
        }

        public override string ToString()
        {
            return "MOUSE_LEAVE_LOG_ENTRY_BOUNDARIES";
        }
    }
}
