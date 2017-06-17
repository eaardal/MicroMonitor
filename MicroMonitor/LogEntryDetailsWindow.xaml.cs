using System;
using System.Windows;

namespace MicroMonitor
{
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
            this.LogDetails.FontSize = AppConfiguration.DetailsWindowFontSize();
            this.Title = $"Log details: {LogEntry.Severity} - {LogEntry.Timestamp:dd.MM.yy HH:mm:ss} - {LogEntry.Source}";
        }
    }
}
