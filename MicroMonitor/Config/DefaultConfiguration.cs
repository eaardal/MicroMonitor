using System;
using MicroMonitor.Model;
using Serilog.Events;

namespace MicroMonitor.Config
{
    public class DefaultConfiguration
    {
        public static string LogEntryColorErrorStale() => "";
        public static LogEventLevel LogLevel() => LogEventLevel.Debug;
        public static string LogEntryColorError() => throw new NotImplementedException();
        public static string LogEntryColorWarningStale() => throw new NotImplementedException();
        public static string LogEntryColorWarning() => throw new NotImplementedException();

        public static string LogEntryColorInfoStale() => throw new NotImplementedException();

        public static string LogEntryColorInfo() => throw new NotImplementedException();

        public static bool LogEntryStaleEnabled() => throw new NotImplementedException();

        public static int LogEntryStaleThresholdInMinutes() => throw new NotImplementedException();

        public static int MainWindowFontSize() => throw new NotImplementedException();

        public static WindowSpawnMethod MainWindowSpawnMethod() => throw new NotImplementedException();

        public static int MainWindowWidth() => throw new NotImplementedException();

        public static int MainWindowHeight() => throw new NotImplementedException();

        public static int DetailsWindowFontSize() => throw new NotImplementedException();

        public static GrowDirection DetailsWindowGrowDirection() => throw new NotImplementedException();

        public static int DetailsWindowHeight() => throw new NotImplementedException();

        public static int DetailsWindowWidth() => throw new NotImplementedException();

        public static int PollIntervalSeconds() => throw new NotImplementedException();
    }
}
