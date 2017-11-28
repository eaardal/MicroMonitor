namespace MicroMonitor.Services
{
    public interface IEventLogPoller
    {
        event AfterEventLogPoll OnAfterEventLogPoll;
        void StartPollingAtIntervals(string logName, double pollIntervalSeconds);
        void StopPolling();
    }
}