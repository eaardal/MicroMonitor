using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;

namespace MicroMonitor
{
    public partial class MainWindow : Window
    {
        private readonly EventViewerReader _eventViewerReader = new EventViewerReader();
        private readonly EventViewerPoller _eventViewerPoller = new EventViewerPoller();
        private readonly MicroLogRetriever _microLogRetriever = new MicroLogRetriever();
        private readonly Timer _nextReadTimer = new Timer();
        private DateTime _lastReadTime = DateTime.MinValue;
        private DateTime _expectedNextReadTime = DateTime.MinValue;

        public MainWindow()
        {
            InitializeComponent();

            this.HeaderPanel.Visibility = Visibility.Collapsed;

            this.ContentRendered += OnContentRendered;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object o, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.LeftCtrl)
            {
                this.HeaderPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void OnKeyDown(object o, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.LeftCtrl)
            {
                this.HeaderPanel.Visibility = Visibility.Visible;
            }
        }

        private void OnContentRendered(object o, EventArgs eventArgs)
        {
            WindowHelper.PositionWindowAtMouseCursor(this);

            var logName = AppConfiguration.LogName();
            var pollIntervalSeconds = AppConfiguration.PollIntervalSeconds();

            GetInitialEventViewerLogs(logName);
            StartPollingForEventViewerLogs(logName, pollIntervalSeconds);
        }

        private void StartPollingForEventViewerLogs(string logName, int pollIntervalSeconds)
        {
            _eventViewerPoller.StartPollingAtIntervals(logName, pollIntervalSeconds);

            var readInterval = pollIntervalSeconds + 2;

            _expectedNextReadTime = DateTime.Now.AddSeconds(readInterval);

            _nextReadTimer.Interval = 1000;
            _nextReadTimer.Elapsed += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_expectedNextReadTime != DateTime.MinValue)
                    {
                        var nextReadInSeconds = _expectedNextReadTime.Subtract(DateTime.Now).Seconds;
                        this.NextRead.Text = $"Next read: {nextReadInSeconds}s";
                    }
                    else
                    {
                        this.NextRead.Text = $"Next read: TBD";
                    }
                    
                });
            };
            _nextReadTimer.Start();

            _microLogRetriever.RetrieveEvery(logName, readInterval, newLogEntries =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _lastReadTime = DateTime.Now;
                    _expectedNextReadTime = _lastReadTime.AddSeconds(readInterval);

                    this.LogEntries.ItemsSource = newLogEntries;
                    UpdateLastRead();
                });
            });
        }

        private void GetInitialEventViewerLogs(string logName)
        {
            var logEntries = _eventViewerReader.ReadEventViewerLog(logName);
            this.LogEntries.ItemsSource = logEntries;
            UpdateLastRead();
        }

        private void UpdateLastRead()
        {
            this.LastRead.Text = $"Last read: {DateTime.Now:HH:mm:ss}";
        }

        private void OnShowLogEntryDetails(object sender, RoutedEventArgs e)
        {
            var btn = (Button) sender;
            var logEntry = (MicroLogEntry) btn.DataContext;

            var detailsWindow = new LogEntryDetailsWindow {LogEntry = logEntry};
            detailsWindow.Left = this.Left + this.Width;
            detailsWindow.Top = this.Top;
            detailsWindow.Height = this.Height;
            detailsWindow.Show();
        }

        private void OnReadNow(object sender, RoutedEventArgs e)
        {
            var logName = AppConfiguration.LogName();
            GetInitialEventViewerLogs(logName);
        }
    }
}
