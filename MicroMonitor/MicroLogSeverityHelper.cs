using System.Diagnostics;

namespace MicroMonitor
{
    public class MicroLogSeverityHelper
    {
        public static MicroLogSeverity MapSeverity(EventLogEntry eventLogEntry)
        {
            switch (eventLogEntry.EntryType)
            {
                case EventLogEntryType.Error:
                case EventLogEntryType.FailureAudit:
                    return MicroLogSeverity.Error;
                case EventLogEntryType.Information:
                case EventLogEntryType.SuccessAudit:
                    return MicroLogSeverity.Info;
                case EventLogEntryType.Warning:
                    return MicroLogSeverity.Warning;
                default:
                    return MicroLogSeverity.Info;

            }
        }
    }
}