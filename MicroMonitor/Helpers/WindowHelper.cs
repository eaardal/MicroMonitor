using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using MicroMonitor.Infrastructure;
using MicroMonitor.Interop;

namespace MicroMonitor.Helpers
{
    class WindowHelper
    {
        public static (double left, double top) GetMouseCursorPosition(Window window, double windowHeight, double windowWidth)
        {
            var mousePos = MouseInterop.GetPosition();
            Logger.Verbose($@"Mouse X{mousePos.X}, Y{mousePos.Y}");

            // Get the screen area minus the Windows taskbar
            var workingArea = GetScreen(window).WorkingArea;
            var workingAreaAsPoint = RealPixelsToWpf(window, new Point(workingArea.Left, workingArea.Bottom));
            Logger.Verbose($"Working area point {workingAreaAsPoint.X}X {workingAreaAsPoint.Y}Y");

            double yOffset = 0;

            // If the mouse is not in the working area aka the mouse is over the taskbar (the program was launched from a taskbar shortcut)
            if (mousePos.Y > workingAreaAsPoint.Y)
            {
                // Set Y offset to the current mouse position minus the working area bottom point aka the distance between the mouse cursor and the border of the task bar
                yOffset = mousePos.Y - workingAreaAsPoint.Y;

                Logger.Verbose($"yOffset={yOffset}");
            }

            // Set the window's upper left corner position so that it's positioned centered over the mouse cursor
            var left = mousePos.X - windowWidth / 2;
            var top = mousePos.Y - (windowHeight + yOffset);

            Logger.Verbose($"Window positioned at {left} left, {top} top");

            return (left, top);
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

        public static (double left, double top) GetCenterScreenPosition(Window window)
        {
            var workingArea = GetScreen(window).WorkingArea;
            var left = (int)(workingArea.Width / 2) - (window.Width / 2);
            var top = (int) (workingArea.Height / 2) - (window.Height / 2);

            return (left, top);
        }
    }
}
