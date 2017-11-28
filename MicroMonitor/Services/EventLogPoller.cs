using MicroMonitor.Infrastructure;
using Timer = System.Timers.Timer;

namespace MicroMonitor.Services
{
    public delegate void AfterEventLogPoll();

    public class EventLogPoller : IEventLogPoller
    {
        public event AfterEventLogPoll OnAfterEventLogPoll;

        private readonly Timer _timer = new Timer();
        private readonly EventLogReader _eventLogReader = new EventLogReader();
        
        public void StartPollingAtIntervals(string logName, double pollIntervalSeconds)
        {
            _timer.Interval = pollIntervalSeconds * 1000;

            Logger.Debug($"Timer polling Event Log {logName} every {pollIntervalSeconds}s / {_timer.Interval}ms");

            _timer.Elapsed += (sender, args) =>
            {
                var logEntries = _eventLogReader.ReadEventLog(logName);

                EventLogCache.Instance.InsertOrUpdate(logName, logEntries);

                OnAfterEventLogPoll?.Invoke();

                //var thread = new Thread(Poll);
                //thread.Start(new List<object> {logName});

                //Logger.Debug($"Spawned thread {thread.ManagedThreadId} for polling Event Log \"{logName}\"");
            };
            _timer.Start();
        }

        public void StopPolling()
        {
            _timer.Stop();
        }

        //private void Poll(object data)
        //{
        //    (var ok, var logDisplayName) = ParseArguments(data);

        //    if (!ok)
        //    {
        //        Logger.Error("Could not parse arguments passed as object to EventLogPoller.Poll");
        //        return;
        //    }

        //    var logEntries = _eventLogReader.ReadEventLog(logDisplayName);

        //    MicroLogCache.Instance.InsertOrUpdate(logDisplayName, logEntries);
        //}

        //private static (bool ok, string logDisplayName) ParseArguments(object data)
        //{
        //    var argsList = data as List<object>;

        //    if (argsList != null)
        //    {
        //        var logDisplayName = (string)argsList.ElementAt(0);

        //        return (true, logDisplayName);
        //    }

        //    return (false, null);
        //}
    }
}