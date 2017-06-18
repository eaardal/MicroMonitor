using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using MicroMonitor.Interop;

namespace MicroMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
#if DEBUG
            this.Startup += (sender, args) =>
            {
                ConsoleWindow.Create();
            };

            this.Exit += (sender, args) =>
            {
                ConsoleWindow.Destroy();
            };
#endif
        }

    }
}
