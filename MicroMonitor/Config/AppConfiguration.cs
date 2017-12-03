using System;
using System.Configuration;
using MicroMonitor.Model;
using Serilog.Events;

namespace MicroMonitor.Config
{
    public class AppConfiguration
    {
        public static string LogName() =>
            ReadValue<string>("LogName");

        public static int PollIntervalSeconds() =>
            ReadValue("PollIntervalSeconds", DefaultConfiguration.PollIntervalSeconds());

        public static int DetailsWindowWidth() =>
            ReadValue("DetailsWindow.Width", DefaultConfiguration.DetailsWindowWidth());

        public static int DetailsWindowHeight() =>
            ReadValue("DetailsWindow.Height", DefaultConfiguration.DetailsWindowHeight());

        public static GrowDirection DetailsWindowGrowDirection() =>
            ReadValue("DetailsWindow.GrowDirection", DefaultConfiguration.DetailsWindowGrowDirection());

        public static int DetailsWindowFontSize() =>
            ReadValue("DetailsWindow.FontSize", DefaultConfiguration.DetailsWindowFontSize());

        public static int MainWindowHeight() =>
            ReadValue("MainWindow.Height", DefaultConfiguration.MainWindowHeight());

        public static int MainWindowWidth() =>
            ReadValue("MainWindow.Width", DefaultConfiguration.MainWindowWidth());

        public static WindowSpawnMethod MainWindowSpawnMethod() =>
            ReadValue("MainWindow.SpawnMethod", DefaultConfiguration.MainWindowSpawnMethod());

        public static int MainWindowFontSize() =>
            ReadValue("MainWindow.FontSize", DefaultConfiguration.MainWindowFontSize());

        public static int LogEntryStaleThresholdInMinutes() =>
            ReadValue("LogEntry.Stale.ThresholdInMinutes", DefaultConfiguration.LogEntryStaleThresholdInMinutes());

        public static bool LogEntryStaleEnabled() =>
            ReadValue("LogEntry.Stale.Enabled", DefaultConfiguration.LogEntryStaleEnabled());

        public static string LogEntryColorInfo() =>
            ReadValue("LogEntry.Color.Info", DefaultConfiguration.LogEntryColorInfo());

        public static string LogEntryColorInfoStale() =>
            ReadValue("LogEntry.Color.Info.Stale", DefaultConfiguration.LogEntryColorInfoStale());

        public static string LogEntryColorWarning() =>
            ReadValue("LogEntry.Color.Warning", DefaultConfiguration.LogEntryColorWarning());

        public static string LogEntryColorWarningStale() =>
            ReadValue("LogEntry.Color.Warning.Stale", DefaultConfiguration.LogEntryColorWarningStale());

        public static string LogEntryColorError() =>
            ReadValue("LogEntry.Color.Error", DefaultConfiguration.LogEntryColorError());

        public static string LogEntryColorErrorStale() =>
            ReadValue("LogEntry.Color.Error.Stale", DefaultConfiguration.LogEntryColorErrorStale());

        public static LogEventLevel LogLevel() =>
            ReadValue("SerilogLogLevel", DefaultConfiguration.LogLevel());

        private static T ReadValue<T>(string key)
        {
            return ReadValue(key, default(T), true);
        }

        private static T ReadValue<T>(string key, T defaultValue, bool throwOnException = false)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return (T) Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                if (throwOnException)
                {
                    throw;
                }
                return defaultValue;
            }
        }
    }
}
