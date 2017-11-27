using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using MicroMonitor.Engine.EventLog;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Services
{
    class EventLogPollingCoordinator
    {
        private readonly IAppStore _store;
        private readonly IEventLogPoller _eventLogPoller;
        private readonly Timer _nextReadTimer = new Timer();
        private DateTime _lastReadTime = DateTime.MinValue;
        private DateTime _expectedNextReadTime = DateTime.MinValue;

        public EventLogPollingCoordinator(IEventLogPoller eventLogPoller, IAppStore store)
        {
            _store = store;
            _eventLogPoller = eventLogPoller ?? throw new ArgumentNullException(nameof(eventLogPoller));
        }

        public void Start(string logName, int pollIntervalSeconds)
        {
            _eventLogPoller.StartPollingAtIntervals(logName, pollIntervalSeconds);

            var readInterval = pollIntervalSeconds + 2;

            _expectedNextReadTime = DateTime.Now.AddSeconds(readInterval);

            _nextReadTimer.Interval = 1000;
            _nextReadTimer.Elapsed += (sender, args) =>
            {
                await _store.Dispatch(new SetNextReadText())

                if (_expectedNextReadTime != DateTime.MinValue)
                {
                    var nextReadInSeconds = _expectedNextReadTime.Subtract(DateTime.Now).Seconds;
                    Model.NextReadText = $"Next read: {nextReadInSeconds}s";
                }
                else
                {
                    Model.NextReadText = "Next read: TBD";
                }
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
    }
}
