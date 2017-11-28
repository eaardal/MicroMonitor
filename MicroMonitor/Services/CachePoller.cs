using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    class CachePoller : ICachePoller
    {
        private readonly IEventLogCache _eventLogCache;
        private readonly Timer _timer = new Timer();

        public CachePoller(IEventLogCache eventLogCache)
        {
            _eventLogCache = eventLogCache ?? throw new ArgumentNullException(nameof(eventLogCache));
        }
        
        public void ReadOnInterval(string logName, int seconds, Action<IEnumerable<MicroLogEntry>> onRetrieved)
        {
            _timer.Interval = seconds * 1000;
            
            _timer.Elapsed += (sender, args) =>
            {
                var logEntries = _eventLogCache.Get(logName).ToArray();

                Logger.Debug($"Read {logEntries.Length} log entries from {nameof(EventLogCache)}");

                onRetrieved(logEntries);
            };

            _timer.Start();

            Logger.Info($"Timer polling {nameof(EventLogCache)} every {seconds}s / {_timer.Interval}ms started");
        }
    }
}
