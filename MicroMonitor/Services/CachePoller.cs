using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    delegate Task CachePolled(CachePollResult result);

    internal class CachePollResult
    {
        public IEnumerable<MicroLogEntry> LogEntries { get; set; }
        public double Interval { get; set; }
        public string LogName { get; set; }
    }

    class CachePoller : ICachePoller
    {
        private readonly IEventLogCache _eventLogCache;
        private readonly Timer _timer = new Timer();

        public event CachePolled CachePolled;

        public CachePoller(IEventLogCache eventLogCache)
        {
            _eventLogCache = eventLogCache ?? throw new ArgumentNullException(nameof(eventLogCache));
        }
        
        public void ReadOnInterval(string logName, int seconds)
        {
            _timer.Interval = seconds * 1000;
            
            _timer.Elapsed += (sender, args) =>
            {
                var logEntries = _eventLogCache.Get(logName).ToArray();

                Logger.Debug($"Read {logEntries.Length} log entries from {nameof(EventLogCache)}");

                CachePolled?.Invoke(new CachePollResult
                {
                    LogEntries = logEntries,
                    LogName = logName,
                    Interval = seconds
                });
            };

            _timer.Start();

            Logger.Info($"Timer polling {nameof(EventLogCache)} every {seconds}s / {_timer.Interval}ms started");
        }
    }
}
