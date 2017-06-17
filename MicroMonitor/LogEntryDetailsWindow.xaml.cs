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
            this.DataContext = LogEntry;
            this.LogDetails.Text = LogEntry.Message;
            this.LogDetails.FontSize = AppConfiguration.DetailsWindowFontSize();
            this.Title = $"Log details: {LogEntry.Severity} - {LogEntry.Timestamp:dd.MM.yy HH:mm:ss} - {LogEntry.Source}";
            this.TimestampValue.Text = this.LogEntry.Timestamp.ToString("dd.MM.yy HH:mm:ss");
            this.SourceValue.Text = $"{this.LogEntry.LogName}/{this.LogEntry.Source}";
        }
    }
}
