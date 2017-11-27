using System.Windows;
using MicroMonitor.Model;

namespace MicroMonitor.Views.MainView
{
    class PeekWindow
    {
        private static string _peekWindowId;
        private static Window _peekWindow;

        public static void Open(Window parent, MicroLogEntry logEntry, bool fullscreen = false)
        {
            if (_peekWindow != null && _peekWindowId != logEntry.Id)
            {
                _peekWindow?.Close();

                ShowPeekWindow(parent, logEntry, fullscreen);
                parent.Focus();
            }

            if (_peekWindow == null)
            {
                ShowPeekWindow(parent, logEntry, fullscreen);
                parent.Focus();
            }
        }

        public static void ShowPeekWindow(Window parent, MicroLogEntry logEntry, bool fullscreen = false)
        {
            _peekWindow = DetailsWindow.CreateDetailsWindow(parent, logEntry);
            _peekWindow.Show();

            if (fullscreen)
            {
                _peekWindow.WindowState = WindowState.Maximized;
            }

            _peekWindowId = logEntry.Id;
        }
    }
}
