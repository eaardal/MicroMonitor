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
            CreateConsoleWindowIfDebugging();

            Bootstrapper.Wire();

            Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                Logger.Error(args.Exception, "Dispatcher unhandled exception");
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Logger.Error(args.ExceptionObject as Exception, "App domain unhandled exception");
            };
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            DestroyConsoleWindowIfDebugging();

            base.OnExit(e);
        }

        private static void DestroyConsoleWindowIfDebugging()
        {
#if DEBUG
            ConsoleWindow.Destroy();
#endif
        }

        private static void CreateConsoleWindowIfDebugging()
        {
#if DEBUG
            ConsoleWindow.Create();
#endif
        }
    }
}
