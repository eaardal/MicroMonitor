using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MicroMonitor.Config;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Views.MainView
{
    class MainWindowViewModel
    {
        private readonly MainWindow _window;

        public MainWindowModel Model { get; }

        public MainWindowViewModel(MainWindow window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));

            Model = new MainWindowModel();

            Logger.Create();
        }

        public void OnLoaded(object o, RoutedEventArgs routedEventArgs)
        {
            SetWindowWidthAndHeight();
            SetWindowPosition();
        }

        private void SetWindowPosition()
        {
            var spawnMethod = AppConfiguration.MainWindowSpawnMethod();

            switch (spawnMethod)
            {
                case WindowSpawnMethod.Cursor:
                    (Model.WindowLeft, Model.WindowTop) = WindowHelper.PositionWindowAtMouseCursor(_window, Model.WindowHeight, Model.WindowWidth);
                    break;
                case WindowSpawnMethod.CenterScreen:
                    (Model.WindowLeft, Model.WindowTop) = WindowHelper.PositionWindowAtCenterScreen(_window);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetWindowWidthAndHeight()
        {
            Model.WindowWidth = AppConfiguration.MainWindowWidth();
            Model.WindowHeight = AppConfiguration.MainWindowHeight();
        }

        public void DisableCloseAllDetailWindowsButton()
        {
            Model.IsCloseAllDetailWindowsButtonEnabled = false;
        }
    }
}
