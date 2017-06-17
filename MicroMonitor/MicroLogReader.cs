﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace MicroMonitor
{
    class MicroLogReader
    {
        private readonly Timer _timer = new Timer();
        
        public void ReadOnInterval(string logName, int seconds, Action<IEnumerable<MicroLogEntry>> onRetrieved)
        {
            _timer.Interval = seconds * 1000;
            
            _timer.Elapsed += (sender, args) =>
            {
                var logEntries = MicroLogCache.Instance.Get(logName).ToArray();

                Logger.Info($"Read {logEntries.Length} log entries from {nameof(MicroLogCache)}");

                onRetrieved(logEntries);
            };

            _timer.Start();

            Logger.Info($"Timer polling {nameof(MicroLogCache)} every {seconds}s / {_timer.Interval}ms started");
        }
    }
}
