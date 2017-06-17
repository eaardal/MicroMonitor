using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MicroMonitor
{
    class EventLogReader
    {
        public IEnumerable<MicroLogEntry> ReadEventLog(string logName)
        {
            if (!EventLog.Exists(logName))
            {
                Logger.Info($"Event Log \"{logName}\" does not exist");
                return new List<MicroLogEntry>();
            }

            var allEventLogs = EventLog.GetEventLogs();

            var eventLog = allEventLogs.First(log => log.LogDisplayName == logName);

            var eventLogEntriesBuffer = new EventLogEntry[eventLog.Entries.Count];

            eventLog.Entries.CopyTo(eventLogEntriesBuffer, 0);

            Logger.Info($"Read {eventLogEntriesBuffer.Length} log entries from Event Log \"{logName}\"");

            return eventLogEntriesBuffer.Select(l => new MicroLogEntry
            {
                Id = $"{l.TimeGenerated.Ticks}-{l.EntryType.ToString()}",
                Source = l.Source,
                Message = l.Message,
                Severity = MicroLogSeverityHelper.MapSeverity(l),
                Timestamp = l.TimeWritten,
                LogName = logName
            }).OrderByDescending(l => l.Timestamp);
        }
    }
}
