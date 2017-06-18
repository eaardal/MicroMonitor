using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using MicroMonitor.Infrastructure;
using MicroMonitor.Interop;

namespace MicroMonitor.Helpers
{
    class WindowHelper
    {
        public static void PositionWindowAtMouseCursor(Window window)
        {
            var mousePos = MouseInterop.GetPosition();
            Logger.Info($@"Mouse X{mousePos.X}, Y{mousePos.Y}");

            // Get the screen area minus the Windows taskbar
            var workingArea = GetScreen(window).WorkingArea;
            var workingAreaAsPoint = RealPixelsToWpf(window, new Point(workingArea.Left, workingArea.Bottom));
            Logger.Info($"Working area point {workingAreaAsPoint.X}X {workingAreaAsPoint.Y}Y");

            double yOffset = 0;

            // If the mouse is not in the working area aka the mouse is over the taskbar (the program was launched from a taskbar shortcut)
            if (mousePos.Y > workingAreaAsPoint.Y)
            {
                // Set Y offset to the current mouse position minus the working area bottom point aka the distance between the mouse cursor and the border of the task bar
                yOffset = mousePos.Y - workingAreaAsPoint.Y;

                Logger.Info($"yOffset={yOffset}");
            }

            // Set the window's upper left corner position so that it's positioned centered over the mouse cursor
            window.Left = mousePos.X - window.Width / 2;
            window.Top = mousePos.Y - (window.Height + yOffset);

            Logger.Info($"Window positioned at {window.Left} left, {window.Top} top");
        }

        private static Screen GetScreen(Window window)
        {
            return Screen.FromHandle(new WindowInteropHelper(window).Handle);
        }

        private static Point RealPixelsToWpf(Window w, Point p)
        {
            var presentationSource = PresentationSource.FromVisual(w);

            if (presentationSource != null)
            {
                if (presentationSource.CompositionTarget != null)
                {
                    var t = presentationSource.CompositionTarget.TransformFromDevice;

                    return t.Transform(p);
                }
                return default(Point);
            }
            return default(Point);
        }

        public static void PositionWindowAtCenterScreen(Views.MainView.MainWindow window)
        {
            var workingArea = GetScreen(window).WorkingArea;
            window.Left = (int)(workingArea.Width / 2) - (window.Width / 2);
            window.Top = (int) (workingArea.Height / 2) - (window.Height / 2);
        }
    }
}
