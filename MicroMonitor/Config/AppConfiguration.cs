using System;
using System.Configuration;
using MicroMonitor.Model;
using Serilog.Events;

namespace MicroMonitor.Config
{
    class AppConfiguration
    {
        public static string LogName() =>
            ConfigurationManager.AppSettings["LogName"];

        public static int PollIntervalSeconds() =>
            int.Parse(ConfigurationManager.AppSettings["PollIntervalSeconds"]);

        public static int DetailsWindowWidth() =>
            int.Parse(ConfigurationManager.AppSettings["DetailsWindow.Width"]);

        public static int DetailsWindowHeight() =>
            int.Parse(ConfigurationManager.AppSettings["DetailsWindow.Height"]);

        public static GrowDirection DetailsWindowGrowDirection() =>
            (GrowDirection) Enum.Parse(typeof(GrowDirection),
                ConfigurationManager.AppSettings["DetailsWindow.GrowDirection"]);

        public static int DetailsWindowFontSize() =>
            int.Parse(ConfigurationManager.AppSettings["DetailsWindow.FontSize"]);

        public static int MainWindowHeight() =>
            int.Parse(ConfigurationManager.AppSettings["MainWindow.Height"]);

        public static int MainWindowWidth() =>
            int.Parse(ConfigurationManager.AppSettings["MainWindow.Width"]);

        public static WindowSpawnMethod MainWindowSpawnMethod() =>
            (WindowSpawnMethod) Enum.Parse(typeof(WindowSpawnMethod),
                ConfigurationManager.AppSettings["MainWindow.SpawnMethod"]);

        public static int MainWindowFontSize() =>
            int.Parse(ConfigurationManager.AppSettings["MainWindow.FontSize"]);

        public static int LogEntryStaleThresholdInMinutes() =>
            int.Parse(ConfigurationManager.AppSettings["LogEntry.Stale.ThresholdInMinutes"]);

        public static bool LogEntryStaleEnabled() =>
            bool.Parse(ConfigurationManager.AppSettings["LogEntry.Stale.Enabled"]);

        public static string LogEntryColorInfo() =>
            ConfigurationManager.AppSettings["LogEntry.Color.Info"];

        public static string LogEntryColorInfoStale() =>
            ConfigurationManager.AppSettings["LogEntry.Color.Info.Stale"];

        public static string LogEntryColorWarning() =>
            ConfigurationManager.AppSettings["LogEntry.Color.Warning"];

        public static string LogEntryColorWarningStale() =>
            ConfigurationManager.AppSettings["LogEntry.Color.Warning.Stale"];

        public static string LogEntryColorError() =>
            ConfigurationManager.AppSettings["LogEntry.Color.Error"];

        public static string LogEntryColorErrorStale() =>
            ConfigurationManager.AppSettings["LogEntry.Color.Error.Stale"];

        public static LogEventLevel LogLevel() =>
            (LogEventLevel) Enum.Parse(typeof(LogEventLevel),
                ConfigurationManager.AppSettings["SerilogLogLevel"]);
    }
}
