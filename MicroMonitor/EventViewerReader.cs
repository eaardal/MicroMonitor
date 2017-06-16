using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MicroMonitor
{
    class EventViewerReader
    {
        public IEnumerable<MicroLogEntry> ReadEventViewerLog(string logName)
        {
            if (!EventLog.Exists(logName)) return new List<MicroLogEntry>();

            Logger.Info($"Event log {logName} exists");

            var allEventLogs = EventLog.GetEventLogs();

            var eventLog = allEventLogs.First(log => log.LogDisplayName == logName);

            var eventLogEntriesBuffer = new EventLogEntry[eventLog.Entries.Count];

            eventLog.Entries.CopyTo(eventLogEntriesBuffer, 0);

            return eventLogEntriesBuffer.Select(l => new MicroLogEntry
            {
                Source = l.Source,
                Message = l.Message,
                Severity = MicroLogSeverityHelper.MapSeverity(l),
                Timestamp = l.TimeWritten
            }).OrderByDescending(l => l.Timestamp);
        }
    }
}
