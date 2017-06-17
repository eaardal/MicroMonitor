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
            if (!EventLog.Exists(logName)) return new List<MicroLogEntry>();

            Logger.Info($"Event log {logName} exists");

            var allEventLogs = EventLog.GetEventLogs();

            var eventLog = allEventLogs.First(log => log.LogDisplayName == logName);

            var eventLogEntriesBuffer = new EventLogEntry[eventLog.Entries.Count];

            eventLog.Entries.CopyTo(eventLogEntriesBuffer, 0);

            return eventLogEntriesBuffer.Select(l => new MicroLogEntry
            {
                Id = $"{l.TimeGenerated.Ticks}-{l.EntryType.ToString()}",
                Source = l.Source,
                Message = l.Message,
                Severity = MicroLogSeverityHelper.MapSeverity(l),
                Timestamp = l.TimeWritten,
                LogName = logName,
                Meta =
                {
                    ReadFromEventLogTimestamp = DateTime.Now
                }
            }).OrderByDescending(l => l.Timestamp);
        }
    }
}
