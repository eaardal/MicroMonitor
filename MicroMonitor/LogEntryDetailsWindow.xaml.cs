using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MicroMonitor
{
    /// <summary>
    /// Interaction logic for LogEntryDetailsWindow.xaml
    /// </summary>
    public partial class LogEntryDetailsWindow : Window
    {
        public MicroLogEntry LogEntry { get; set; }

        public LogEntryDetailsWindow()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            this.LogDetails.Text = LogEntry.Message;
        }
    }
}
