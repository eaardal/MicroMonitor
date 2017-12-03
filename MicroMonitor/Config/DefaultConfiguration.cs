using MicroMonitor.Model;
using Serilog.Events;

namespace MicroMonitor.Config
{
    public class DefaultConfiguration
    {
        public static string LogEntryColorErrorStale() => "";

        public static LogEventLevel LogLevel() => LogEventLevel.Debug;

        public static string LogEntryColorError() => ColorConstants.ErrorColor;

        public static string LogEntryColorWarningStale() => ColorConstants.WarningColorStale;

        public static string LogEntryColorWarning() => ColorConstants.WarningColor;

        public static string LogEntryColorInfoStale() => ColorConstants.InfoColorStale;

        public static string LogEntryColorInfo() => ColorConstants.InfoColor;

        public static bool LogEntryStaleEnabled() => true;

        public static int LogEntryStaleThresholdInMinutes() => 30;

        public static int MainWindowFontSize() => 11;

        public static WindowSpawnMethod MainWindowSpawnMethod() => WindowSpawnMethod.Cursor;

        public static int MainWindowWidth() => 200;

        public static int MainWindowHeight() => 300;

        public static int DetailsWindowFontSize() => 11;

        public static GrowDirection DetailsWindowGrowDirection() => GrowDirection.Up;

        public static int DetailsWindowHeight() => MainWindowHeight() + 200;

        public static int DetailsWindowWidth() => 600;

        public static int PollIntervalSeconds() => 10;
    }
}
