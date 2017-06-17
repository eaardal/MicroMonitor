using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private string _peekWindowId;
        private Window _peekWindow;

        public MainWindow()
        {
            InitializeComponent();

            this.HeaderPanel.Visibility = Visibility.Collapsed;

            this.ContentRendered += OnContentRendered;
            this.Loaded += OnLoaded;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
        }

        private void OnLoaded(object o, RoutedEventArgs routedEventArgs)
        {
            this.Width = AppConfiguration.MainWindowWidth();
            this.Height = AppConfiguration.MainWindowHeight();

            var startupLoc = AppConfiguration.MainWindowSpawnMethod();
            switch (startupLoc)
            {
                case WindowSpawnMethod.Cursor:
                    WindowHelper.PositionWindowAtMouseCursor(this);
                    break;
                case WindowSpawnMethod.CenterScreen:
                    WindowHelper.PositionWindowAtCenterScreen(this);
                    break;
            }

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
            var btn = (Button)sender;
            var logEntry = (MicroLogEntry)btn.DataContext;

            ShowDetailsWindow(logEntry);
        }

        private Window ShowDetailsWindow(MicroLogEntry logEntry)
        {
            var configuredHeight = AppConfiguration.DetailsWindowHeight();
            var height = configuredHeight > 0 ? configuredHeight : this.Height;

            var top = AppConfiguration.DetailsWindowGrowDirection() == GrowDirection.Down
                ? this.Top
                : this.Top + (this.Height - height);

            var detailsWindow = new LogEntryDetailsWindow
            {
                LogEntry = logEntry,
                Left = this.Left + this.Width,
                Top = top,
                Height = height,
                Width = AppConfiguration.DetailsWindowWidth()
            };

            detailsWindow.Show();

            return detailsWindow;
        }

        private void OnReadNow(object sender, RoutedEventArgs e)
        {
            var logName = AppConfiguration.LogName();
            GetInitialEventViewerLogs(logName);
        }

        private void OnMouseOverLogEntry(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = sender as TextBlock;

            if (textBlock == null)
            {
                Logger.Info("Could not cast to text block");
                return;
            }

            var logEntry = textBlock.DataContext as MicroLogEntry;

            if (logEntry == null)
            {
                Logger.Info("Could not cast to log entry");
                return;
            }

            var isMouseOver = (bool)e.NewValue;

            if (isMouseOver && KeyboardFacade.IsLeftShiftDown() && _peekWindowId != logEntry.Id)
            {
                _peekWindow?.Close();

                _peekWindow = ShowDetailsWindow(logEntry);
                _peekWindowId = logEntry.Id;
            }
        }
    }
}
