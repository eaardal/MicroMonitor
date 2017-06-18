using System;
using System.Windows;
using MicroMonitor.Config;
using MicroMonitor.Model;
using Newtonsoft.Json;

namespace MicroMonitor.Views.DetailsView
{
    public partial class LogEntryDetailsWindow : Window
    {
        public MicroLogEntry LogEntry { get; set; }
        private bool _isInfoPanelVisible = false;

        public LogEntryDetailsWindow()
        {
            InitializeComponent();
            CollapseInfoPanel();
        }

        protected override void OnActivated(EventArgs e)
        {
            this.DataContext = LogEntry;
            this.LogDetails.Text = LogEntry.Message;
            this.LogDetails.FontSize = AppConfiguration.DetailsWindowFontSize();
            this.Title = $"{LogEntry.Severity} {LogEntry.Timestamp:HH:mm:ss}";
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

        private void OnToggleInfoPanel(object sender, RoutedEventArgs e)
        {
            ToggleInfoPanelVisibility();
        }

        private void ToggleInfoPanelVisibility()
        {
            if (_isInfoPanelVisible)
            {
                CollapseInfoPanel();
            }
            else
            {
                this.BtnToggleInfoPanel.Content = "hide";

                this.SourceLabel.Visibility = Visibility.Visible;
                this.SourceValue.Visibility = Visibility.Visible;
                this.TimestampLabel.Visibility = Visibility.Visible;
                this.TimestampValue.Visibility = Visibility.Visible;
            }

            _isInfoPanelVisible = !_isInfoPanelVisible;
        }

        private void CollapseInfoPanel()
        {
            this.BtnToggleInfoPanel.Content = "details";

            this.SourceLabel.Visibility = Visibility.Collapsed;
            this.SourceValue.Visibility = Visibility.Collapsed;
            this.TimestampLabel.Visibility = Visibility.Collapsed;
            this.TimestampValue.Visibility = Visibility.Collapsed;
        }
    }
}
