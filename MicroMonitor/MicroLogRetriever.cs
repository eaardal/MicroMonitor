using System;
using System.Collections.Generic;
using System.Timers;

namespace MicroMonitor
{
    class MicroLogRetriever
    {
        private readonly Timer _timer = new Timer();
        
        public void RetrieveEvery(string logName, int seconds, Action<IEnumerable<MicroLogEntry>> onRetrieved)
        {
            _timer.Interval = seconds * 1000;
            _timer.Elapsed += (sender, args) =>
            {
                var logEntries = MicroLogCache.Instance.Get(logName);
                onRetrieved(logEntries);
            };
            _timer.Start();
        }
    }
}
