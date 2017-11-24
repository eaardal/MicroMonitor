using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicroMonitor.Views.MainView
{
    class MainWindowModel
    {
        public Visibility HeaderPanelVisibility { get; set; } = Visibility.Collapsed;
        public bool IsCloseAllDetailWindowsButtonEnabled { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public double WindowLeft { get; set; }
        public double WindowTop { get; set; }
    }
}
