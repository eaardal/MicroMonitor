﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MicroMonitor.Config;
using MicroMonitor.Engine.EventLog;
using MicroMonitor.Engine.MicroLog;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Views.DetailsView;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;

namespace MicroMonitor.Views.MainView
{
    public partial class MainWindow : Window
    {
        private readonly EventLogReader _eventLogReader = new EventLogReader();
        private readonly EventLogPoller _eventLogPoller = new EventLogPoller();
        private readonly MicroLogReader _microLogReader = new MicroLogReader();
        private readonly Timer _nextReadTimer = new Timer();
        private DateTime _lastReadTime = DateTime.MinValue;
        private DateTime _expectedNextReadTime = DateTime.MinValue;
        private string _peekWindowId;
        private Window _peekWindow;
        private readonly List<Window> _openDetailWindows = new List<Window>();

        public MainWindow()
        {
            Logger.Create();

            InitializeComponent();

            HeaderPanel.Visibility = Visibility.Collapsed;
            BtnCloseAllDetailWindows.IsEnabled = false;

            ContentRendered += OnContentRendered;
            Loaded += OnLoaded;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }
        
        private void OnLoaded(object o, RoutedEventArgs routedEventArgs)
        {
            Width = AppConfiguration.MainWindowWidth();
            Height = AppConfiguration.MainWindowHeight();

            var spawnMethod = AppConfiguration.MainWindowSpawnMethod();

            switch (spawnMethod)
            {
                case WindowSpawnMethod.Cursor:
                    WindowHelper.PositionWindowAtMouseCursor(this);
                    break;
                case WindowSpawnMethod.CenterScreen:
                    WindowHelper.PositionWindowAtCenterScreen(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnKeyUp(object o, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.LeftCtrl)
            {
                HeaderPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void OnKeyDown(object o, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                HeaderPanel.Visibility = Visibility.Visible;
            }

            if (e.Key == Key.LeftShift)
            {
                var ele = Mouse.DirectlyOver as UIElement;
                
                var textBlock = ele as TextBlock;

                if (textBlock != null)
                {
                    Logger.Debug("Mouse is over TextBlock");

                    var logEntry = textBlock.DataContext as MicroLogEntry;

                    if (logEntry == null)
                    {
                        Logger.Debug($"Could not cast TextBlock.DataContext to {typeof(MicroLogEntry).FullName}");
                        return;
                    }

                    OpenPeekWindow(logEntry);
                }
                else
                {
                    Logger.Debug("Mouse is not over TextBlock");
                }
            }
        }

        private void OpenPeekWindow(MicroLogEntry logEntry, bool fullscreen = false)
        {
            if (_peekWindow != null && _peekWindowId != logEntry.Id)
            {
                _peekWindow?.Close();

                ShowPeekWindow(logEntry, fullscreen);
                Focus();
            }

            if (_peekWindow == null)
            {
                ShowPeekWindow(logEntry, fullscreen);
                Focus();
            }
        }

        private void ShowPeekWindow(MicroLogEntry logEntry, bool fullscreen = false)
        {
            _peekWindow = CreateDetailsWindow(logEntry);
            _peekWindow.Show();

            if (fullscreen)
            {
                _peekWindow.WindowState = WindowState.Maximized;
            }

            _peekWindowId = logEntry.Id;
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
                        NextRead.Text = $"Next read: {nextReadInSeconds}s";
                    }
                    else
                    {
                        NextRead.Text = "Next read: TBD";
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

                    GroupAndBindLogEntries(newLogEntries);
                    UpdateLastRead();
                });
            });
        }

        private void GetInitialEventViewerLogs(string logName)
        {
            var logEntries = _eventLogReader.ReadEventLog(logName).ToArray();
            GroupAndBindLogEntries(logEntries);
            
            UpdateLastRead();
        }

        private void GroupAndBindLogEntries(IEnumerable<MicroLogEntry> logEntries)
        {
            var grouped = logEntries.GroupBy(e => e.Timestamp.Date).Select(grp => new
            {
                Key = grp.Key.Date == DateTime.Today ? "Today" : grp.Key.ToString("dd.MM.yy"),
                LogEntries = grp.Select(e => e)
            });

            LogEntries.ItemsSource = grouped;
        }

        private void UpdateLastRead()
        {
            LastRead.Text = $"Last read: {DateTime.Now:HH:mm:ss}";
        }

        private void OnShowLogEntryDetails(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var logEntry = (MicroLogEntry)btn.DataContext;

            OpenNewDetailsWindow(logEntry);
        }

        private void OpenNewDetailsWindow(MicroLogEntry logEntry)
        {
            var detailsWindow = CreateDetailsWindow(logEntry);
            detailsWindow.Show();

            _openDetailWindows.Add(detailsWindow);

            BtnCloseAllDetailWindows.IsEnabled = _openDetailWindows.Any();
        }

        private Window CreateDetailsWindow(MicroLogEntry logEntry)
        {
            var configuredHeight = AppConfiguration.DetailsWindowHeight();
            var height = configuredHeight > 0 ? configuredHeight : Height;

            var top = AppConfiguration.DetailsWindowGrowDirection() == GrowDirection.Down
                ? Top
                : Top + (Height - height);

            const int marginBuffer = 20;

            var detailsWindow = new LogEntryDetailsWindow
            {
                LogEntry = logEntry,
                Left = Left + Width + marginBuffer,
                Top = top,
                Height = height,
                Width = AppConfiguration.DetailsWindowWidth()
            };
            
            return detailsWindow;
        }

        private void OnReadNow(object sender, RoutedEventArgs e)
        {
            var logName = AppConfiguration.LogName();
            GetInitialEventViewerLogs(logName);
        }
        
        private void OnCloseAllDetailWindows(object sender, RoutedEventArgs e)
        {
            foreach (var openDetailWindow in _openDetailWindows)
            {
                openDetailWindow.Close();
            }

            _openDetailWindows.Clear();

            BtnCloseAllDetailWindows.IsEnabled = false;
        }

        private void OnLogEntryClick(object sender, MouseButtonEventArgs e)
        {
            var stackPanel = (StackPanel) sender;
            var logEntry = (MicroLogEntry) stackPanel.DataContext;

            if (e.ChangedButton == MouseButton.Left)
            {
                OpenPeekWindow(logEntry);
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                OpenPeekWindow(logEntry, true);
            }
        }
    }
}
