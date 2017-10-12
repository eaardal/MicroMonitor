using System;
using System.Windows;
using MicroMonitor.Infrastructure;
using MicroMonitor.Interop;

namespace MicroMonitor.Views
{
    public partial class App
    {
        public App()
        {
#if DEBUG
            LoadCompleted += (sender, args) =>
            {
                ConsoleWindow.Create();
            };

            Exit += (sender, args) =>
            {
                ConsoleWindow.Destroy();
            };
#endif

            Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                Logger.Error(args.Exception, "Dispatcher unhandled exception");
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Logger.Error(args.ExceptionObject as Exception, "App domain unhandled exception");
            };
        }
    }
}
