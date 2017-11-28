namespace MicroMonitor.Services
{
    public interface IEventLogPoller
    {
        event EventLogPolled EventLogPolled;
        void StartPollingAtIntervals(string logName, double pollIntervalSeconds);
        void StopPolling();
    }
}