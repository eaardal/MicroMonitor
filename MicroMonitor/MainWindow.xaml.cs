using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private readonly EventLogReader _eventLogReader = new EventLogReader();
        private readonly EventLogPoller _eventLogPoller = new EventLogPoller();
        private readonly MicroLogReader _microLogReader = new MicroLogReader();
        private readonly MicroLogPersistedRegistry _persistedRegistry = new MicroLogPersistedRegistry();
        private readonly Timer _nextReadTimer = new Timer();
        private DateTime _lastReadTime = DateTime.MinValue;
        private DateTime _expectedNextReadTime = DateTime.MinValue;
        private bool _isHoldingLeftShiftDown = false;

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

            if (keyEventArgs.Key == Key.LeftShift)
            {
                _isHoldingLeftShiftDown = false;
            }
        }

        private void OnKeyDown(object o, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.LeftCtrl)
            {
                this.HeaderPanel.Visibility = Visibility.Visible;
            }

            if (keyEventArgs.Key == Key.LeftShift)
            {
                _isHoldingLeftShiftDown = true;
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
            _eventLogPoller.StartPollingAtIntervals(logName, pollIntervalSeconds);

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

            _microLogReader.ReadOnInterval(logName, readInterval, newLogEntries =>
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
            var logEntries = _eventLogReader.ReadEventLog(logName);
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

        private void OnLogEntryMouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = (TextBlock) sender;
            var logEntry = (MicroLogEntry) textBlock.DataContext;

            if (_isHoldingLeftShiftDown)
            {
                _persistedRegistry.Save(logEntry);

                logEntry.Meta.IsMarkedAsRead = true;
                logEntry.Meta.MarkedAsReadTimestamp = DateTime.Now;
            }
        }
    }
}
