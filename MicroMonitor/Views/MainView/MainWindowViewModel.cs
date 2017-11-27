using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Config;
using MicroMonitor.Engine.EventLog;
using MicroMonitor.Engine.MicroLog;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Views.MainView
{
    class MainWindowViewModel
    {
        private readonly IAppStore _store;
        private readonly IMediator _mediator;
        private readonly EventLogReader _eventLogReader = new EventLogReader();
        private readonly EventLogPoller _eventLogPoller = new EventLogPoller();
        private readonly MicroLogReader _microLogReader = new MicroLogReader();
        private readonly MainWindow _window;
        private bool _activatedOnce;
        private readonly Timer _nextReadTimer = new Timer();
        private DateTime _lastReadTime = DateTime.MinValue;
        private DateTime _expectedNextReadTime = DateTime.MinValue;
        private string _peekWindowId;
        private Window _peekWindow;
        private readonly List<Window> _openDetailWindows = new List<Window>();
        private IEnumerable<MicroLogEntry> _logEntries = new List<MicroLogEntry>();
        private int _traversingIndex = -1;

        public MainWindowModel Model { get; }

        public MainWindowViewModel(MainWindow window, IAppStore store)
        {
            _store = store;
            _window = window ?? throw new ArgumentNullException(nameof(window));

            Model = new MainWindowModel();

            Logger.Create();
        }

        public async Task OnKeyUp(object o, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                await _store.Dispatch(new ToggleHeaderPanelVisibility(Visibility.Visible));
            }
        }

        public async Task OnKeyDown(object o, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                await _store.Dispatch(new ToggleHeaderPanelVisibility(Visibility.Collapsed));
            }

            if (e.Key == Key.LeftShift)
            {
                await _store.Dispatch(new OpenPeekWindowUnderMouseCursor(e));
            }

            if (e.Key == Key.R || e.Key == Key.R && KeyboardFacade.IsLeftCtrlDown())
            {
                await _store.Dispatch(new RefreshEventLogEntries());
            }
            
            if (e.Key >= Key.D1 && e.Key <= Key.D9)
            {
                await _store.Dispatch(new OpenPeekWindowForNumericKey(e));
            }

            if (e.Key == Key.S || e.Key == Key.Down)
            {
                await _store.Dispatch(new TraverseDownAndOpenPeekWindow());
            }

            if (e.Key == Key.W || e.Key == Key.Up)
            {
                await _store.Dispatch(new TraverseUpAndOpenPeekWindow());
            }
        }
        
        public async Task OnActivated(object o, EventArgs eventArgs)
        {
            var logName = AppConfiguration.LogName();

            if (!_activatedOnce)
            {
                _activatedOnce = true;

                var pollIntervalSeconds = AppConfiguration.PollIntervalSeconds();

                await GetAndBindEvents(logName);

                StartPollingForEventViewerLogs(logName, pollIntervalSeconds);
            }
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
                        Model.NextReadText = $"Next read: {nextReadInSeconds}s";
                    }
                    else
                    {
                        Model.NextReadText = "Next read: TBD";
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

                    GroupAndBindLogEntries(newLogEntries.ToArray());
                    UpdateLastRead();
                });
            });
        }

        private void GroupAndBindLogEntries(MicroLogEntry[] logEntries)
        {
            _logEntries = logEntries;

            var grouped = logEntries.GroupBy(e => e.Timestamp.Date).Select(grp => new GroupedMicroLogEntry
            {
                Key = grp.Key.Date == DateTime.Today ? "Today" : grp.Key.ToString("dd.MM.yy"),
                LogEntries = new ObservableCollection<MicroLogEntry>(grp.Select(e => e))
            });

            foreach (var grp in grouped)
            {
                Model.GroupedLogEntries.Add(grp);
            }
        }

        private void UpdateLastRead()
        {
            Model.LastReadText = $"Last read: {DateTime.Now:HH:mm:ss}";
        }

        private async Task GetAndBindEvents(string logName)
        {
            var logEntries = await _eventLogReader.ReadEventLogAsync(logName);

            GroupAndBindLogEntries(logEntries.ToArray());

            UpdateLastRead();
        }

        public void OnLoaded(object o, RoutedEventArgs routedEventArgs)
        {
            SetWindowWidthAndHeight();
            SetWindowPosition();
        }

        private void SetWindowPosition()
        {
            var spawnMethod = AppConfiguration.MainWindowSpawnMethod();

            switch (spawnMethod)
            {
                case WindowSpawnMethod.Cursor:
                    (Model.WindowLeft, Model.WindowTop) = WindowHelper.PositionWindowAtMouseCursor(_window, Model.WindowHeight, Model.WindowWidth);
                    break;
                case WindowSpawnMethod.CenterScreen:
                    (Model.WindowLeft, Model.WindowTop) = WindowHelper.PositionWindowAtCenterScreen(_window);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetWindowWidthAndHeight()
        {
            Model.WindowWidth = AppConfiguration.MainWindowWidth();
            Model.WindowHeight = AppConfiguration.MainWindowHeight();
        }

        public void DisableCloseAllDetailWindowsButton()
        {
            Model.IsCloseAllDetailWindowsButtonEnabled = false;
        }
    }
}
