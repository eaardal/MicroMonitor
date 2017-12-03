using System;
using System.Threading.Tasks;
using System.Timers;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Services
{
    class EventLogPollingCoordinator : IEventLogPollingCoordinator
    {
        private readonly ICachePoller _cachePoller;
        private readonly IAppStore _store;
        private readonly IEventLogPoller _eventLogPoller;
        private readonly Timer _nextReadTimer = new Timer();
        private DateTime _lastReadTime = DateTime.MinValue;
        private DateTime _expectedNextReadTime = DateTime.MinValue;

        public EventLogPollingCoordinator(IEventLogPoller eventLogPoller, ICachePoller cachePoller, IAppStore store)
        {
            _cachePoller = cachePoller;
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _eventLogPoller = eventLogPoller ?? throw new ArgumentNullException(nameof(eventLogPoller));
            if (cachePoller == null) throw new ArgumentNullException(nameof(cachePoller));

            cachePoller.CachePolled += CachePollerCachePolled;
        }

        private async Task CachePollerCachePolled(CachePollResult pollResult)
        {
            _lastReadTime = DateTime.Now;
            _expectedNextReadTime = _lastReadTime.AddSeconds(pollResult.Interval);

            await _store.Dispatch(new UpdateEventLogEntries(pollResult.LogName, pollResult.LogEntries));
            await _store.Dispatch(new SetLastReadText($"Last read: {DateTime.Now:HH:mm:ss}"));
        }

        public void Start(string logName, int pollIntervalSeconds)
        {
            _eventLogPoller.StartPollingAtIntervals(logName, pollIntervalSeconds);
            
            var readInterval = pollIntervalSeconds + 2;
            _cachePoller.ReadOnInterval(logName, readInterval);

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
        }
    }
}
