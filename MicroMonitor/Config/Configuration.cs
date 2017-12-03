using System;
using MicroMonitor.Model;
using Serilog.Events;

namespace MicroMonitor.Config
{
    public class Configuration : IAppConfiguration
    {
        public string LogName()
        {
            return AppConfiguration.LogName();
        }

        public int PollIntervalSeconds()
        {
            return AppConfiguration.PollIntervalSeconds();
        }

        public int DetailsWindowWidth()
        {
            return AppConfiguration.DetailsWindowWidth();
        }

        public int DetailsWindowHeight()
        {
            return AppConfiguration.DetailsWindowHeight();
        }

        public GrowDirection DetailsWindowGrowDirection()
        {
            return AppConfiguration.DetailsWindowGrowDirection();
        }

        public int DetailsWindowFontSize()
        {
            return AppConfiguration.DetailsWindowFontSize();
        }

        public int MainWindowHeight()
        {
            return AppConfiguration.MainWindowHeight();
        }

        public int MainWindowWidth()
        {
            return AppConfiguration.MainWindowWidth();
        }

        public WindowSpawnMethod MainWindowSpawnMethod()
        {
            return AppConfiguration.MainWindowSpawnMethod();
        }

        public int MainWindowFontSize()
        {
            return AppConfiguration.MainWindowFontSize();
        }

        public int LogEntryStaleThresholdInMinutes()
        {
            return AppConfiguration.LogEntryStaleThresholdInMinutes();
        }

        public bool LogEntryStaleEnabled()
        {
            return AppConfiguration.LogEntryStaleEnabled();
        }

        public string LogEntryColorInfo()
        {
            return AppConfiguration.LogEntryColorInfo();
        }

        public string LogEntryColorInfoStale()
        {
            return AppConfiguration.LogEntryColorInfoStale();
        }

        public string LogEntryColorWarning()
        {
            return AppConfiguration.LogEntryColorWarning();
        }

        public string LogEntryColorWarningStale()
        {
            return AppConfiguration.LogEntryColorWarningStale();
        }

        public string LogEntryColorError()
        {
            return AppConfiguration.LogEntryColorError();
        }

        public string LogEntryColorErrorStale()
        {
            return AppConfiguration.LogEntryColorErrorStale();
        }

        public LogEventLevel LogLevel()
        {
            return AppConfiguration.LogLevel();
        }
    }
}
