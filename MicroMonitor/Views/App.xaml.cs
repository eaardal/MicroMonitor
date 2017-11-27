using System;
using System.Windows;
using MicroMonitor.Infrastructure;
using MicroMonitor.Interop;

namespace MicroMonitor.Views
{
    public partial class App
    {
        protected override void OnActivated(EventArgs e)
        {
#if DEBUG
            ConsoleWindow.Create();
#endif

            Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                Logger.Error(args.Exception, "Dispatcher unhandled exception");
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Logger.Error(args.ExceptionObject as Exception, "App domain unhandled exception");
            };

            base.OnActivated(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
#if DEBUG
            ConsoleWindow.Destroy();
#endif

            base.OnExit(e);
        }
    }
}
