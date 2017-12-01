namespace MicroMonitor.Services
{
    public interface IEventLogPollingCoordinator
    {
        void Start(string logName, int pollIntervalSeconds);
    }
}