using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;
using MicroMonitor.Services.EventLog;
using MicroMonitor.Services.MicroLog;

namespace MicroMonitor.Services
{
    class EventLogPollingCoordinator
    {
        private readonly IMicroLogReader _microLogReader;
        private readonly IAppStore _store;
        private readonly IEventLogPoller _eventLogPoller;
        private readonly Timer _nextReadTimer = new Timer();
        private DateTime _lastReadTime = DateTime.MinValue;
        private DateTime _expectedNextReadTime = DateTime.MinValue;

        public EventLogPollingCoordinator(IEventLogPoller eventLogPoller, IMicroLogReader microLogReader, IAppStore store)
        {
            _microLogReader = microLogReader ?? throw new ArgumentNullException(nameof(microLogReader));
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _eventLogPoller = eventLogPoller ?? throw new ArgumentNullException(nameof(eventLogPoller));
        }

        public void Start(string logName, int pollIntervalSeconds)
        {
            _eventLogPoller.StartPollingAtIntervals(logName, pollIntervalSeconds);

            var readInterval = pollIntervalSeconds + 2;

            _expectedNextReadTime = DateTime.Now.AddSeconds(readInterval);

            _nextReadTimer.Interval = 1000;
            _nextReadTimer.Elapsed += async (sender, args) =>
            {
                string nextReadText;

                if (_expectedNextReadTime != DateTime.MinValue)
                {
                    var nextReadInSeconds = _expectedNextReadTime.Subtract(DateTime.Now).Seconds;
                    nextReadText = $"Next read: {nextReadInSeconds}s";
                }
                else
                {
                    nextReadText = "Next read: TBD";
                }

                await _store.Dispatch(new SetNextReadText(nextReadText));
            };

            _nextReadTimer.Start();

            _microLogReader.ReadOnInterval(logName, readInterval, async newLogEntries =>
            {
                _lastReadTime = DateTime.Now;
                _expectedNextReadTime = _lastReadTime.AddSeconds(readInterval);

                await _store.Dispatch(new UpdateEventLogEntries(logName, newLogEntries));
                await _store.Dispatch(new SetLastReadText($"Last read: {DateTime.Now:HH:mm:ss}"));
            });
        }
    }
}
