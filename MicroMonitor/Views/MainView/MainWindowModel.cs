using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MicroMonitor.Model;

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
        public string NextReadText { get; set; }
        public string LastReadText { get; set; }
        public ObservableCollection<GroupedMicroLogEntry> GroupedLogEntries { get; set; } = new ObservableCollection<GroupedMicroLogEntry>();
    }
}
