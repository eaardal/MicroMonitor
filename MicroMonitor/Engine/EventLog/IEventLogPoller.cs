namespace MicroMonitor.Engine.EventLog
{
    public interface IEventLogPoller
    {
        void StartPollingAtIntervals(string logName, double pollIntervalSeconds);
        void StopPolling();
    }
}