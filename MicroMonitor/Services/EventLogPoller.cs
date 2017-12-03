using System;
using System.Timers;
using MicroMonitor.Infrastructure;
using Timer = System.Timers.Timer;

namespace MicroMonitor.Services
{
    public delegate void EventLogPolled();

    class EventLogPoller : IEventLogPoller
    {
        private readonly IEventLogCache _eventLogCache;
        public event EventLogPolled EventLogPolled;

        private readonly Timer _timer = new Timer();
        private readonly IEventLogReader _eventLogReader;
        private string _logName;

        public EventLogPoller(IEventLogReader eventLogReader, IEventLogCache eventLogCache)
        {
            _eventLogCache = eventLogCache ?? throw new ArgumentNullException(nameof(eventLogCache));
            _eventLogReader = eventLogReader ?? throw new ArgumentNullException(nameof(eventLogReader));
        }

        private void TimerOnElapsed(object o, ElapsedEventArgs elapsedEventArgs)
        {
            var logEntries = _eventLogReader.ReadEventLog(_logName);

            _eventLogCache.InsertOrUpdate(_logName, logEntries);

            EventLogPolled?.Invoke();

            //var thread = new Thread(Poll);
            //thread.Start(new List<object> {logName});

            //Logger.Debug($"Spawned thread {thread.ManagedThreadId} for polling Event Log \"{logName}\"");
        }

        public void StartPollingAtIntervals(string logName, double pollIntervalSeconds)
        {
            _logName = logName;

            _timer.Interval = pollIntervalSeconds * 1000;
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();

            Logger.Verbose($"Timer polling Event Log {logName} every {pollIntervalSeconds}s / {_timer.Interval}ms");
        }

        public void StopPolling()
        {
            _timer.Elapsed -= TimerOnElapsed;
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