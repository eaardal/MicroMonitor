namespace MicroMonitor.Services
{
    internal interface ICachePoller
    {
        event CachePolled CachePolled;
        void ReadOnInterval(string logName, int seconds);
    }
}