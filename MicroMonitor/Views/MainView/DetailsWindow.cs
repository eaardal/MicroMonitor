using System.Windows;
using MicroMonitor.Config;
using MicroMonitor.Model;
using MicroMonitor.Views.DetailsView;

namespace MicroMonitor.Views.MainView
{
    class DetailsWindow
    {
        public static Window CreateDetailsWindow(Window parent, MicroLogEntry logEntry)
        {
            var configuredHeight = AppConfiguration.DetailsWindowHeight();
            var height = configuredHeight > 0 ? configuredHeight : parent.Height;

            var top = AppConfiguration.DetailsWindowGrowDirection() == GrowDirection.Down
                ? parent.Top
                : parent.Top + (parent.Height - height);

            const int marginBuffer = 20;

            var detailsWindow = new LogEntryDetailsWindow
            {
                LogEntry = logEntry,
                Left = parent.Left + parent.Width + marginBuffer,
                Top = top,
                Height = height,
                Width = AppConfiguration.DetailsWindowWidth()
            };

            return detailsWindow;
        }
    }
}
