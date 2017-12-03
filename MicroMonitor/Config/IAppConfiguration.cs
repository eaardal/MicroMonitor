using MicroMonitor.Model;
using Serilog.Events;

namespace MicroMonitor.Config
{
    public interface IAppConfiguration
    {
        string LogName();
        int PollIntervalSeconds();
        int DetailsWindowWidth();
        int DetailsWindowHeight();
        GrowDirection DetailsWindowGrowDirection();
        int DetailsWindowFontSize();
        int MainWindowHeight();
        int MainWindowWidth();
        WindowSpawnMethod MainWindowSpawnMethod();
        int MainWindowFontSize();
        int LogEntryStaleThresholdInMinutes();
        bool LogEntryStaleEnabled();
        string LogEntryColorInfo();
        string LogEntryColorInfoStale();
        string LogEntryColorWarning();
        string LogEntryColorWarningStale();
        string LogEntryColorError();
        string LogEntryColorErrorStale();
        LogEventLevel LogLevel();
    }
}