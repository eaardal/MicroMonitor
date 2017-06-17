using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Timer = System.Timers.Timer;

namespace MicroMonitor
{
    public class EventLogPoller
    {
        private readonly Timer _timer = new Timer();
        private readonly EventLogReader _eventLogReader = new EventLogReader();
        
        public void StartPollingAtIntervals(string logDisplayName, double pollIntervalSeconds)
        {
            _timer.Interval = pollIntervalSeconds * 1000;
            _timer.Elapsed += (sender, args) =>
            {
                var thread = new Thread(Poll);
                thread.Start(new List<object> {logDisplayName});
            };
            _timer.Start();
        }

        public void StopPolling()
        {
            _timer.Stop();
        }

        private void Poll(object data)
        {
            (var ok, var logDisplayName) = ParseArguments(data);

            if (!ok)
            {
                Logger.Error("Could not parse Poll arguments");
                return;
            }

            var logEntries = _eventLogReader.ReadEventLog(logDisplayName);

            MicroLogCache.Instance.InsertOrUpdate(logDisplayName, logEntries);
        }

        private static (bool ok, string logDisplayName) ParseArguments(object data)
        {
            var argsList = data as List<object>;

            if (argsList != null)
            {
                var logDisplayName = (string)argsList.ElementAt(0);

                return (true, logDisplayName);
            }

            return (false, null);
        }


    }
}