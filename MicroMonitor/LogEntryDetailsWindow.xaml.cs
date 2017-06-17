using System;
using System.Windows;
using Newtonsoft.Json;

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

        private void OnCopyMessageToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(LogEntry.Message);
        }

        private void OnCopyLogEntryAsJson(object sender, RoutedEventArgs e)
        {
            var json = JsonConvert.SerializeObject(LogEntry, Formatting.Indented);
            Clipboard.SetText(json);
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
