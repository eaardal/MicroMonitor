namespace MicroMonitor.Services
{
    public interface ICachePoller
    {
        event CachePolled CachePolled;
        void ReadOnInterval(string logName, int seconds);
    }
}