﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Engine.EventLog
{
    class EventLogReader
    {
        public IEnumerable<MicroLogEntry> ReadEventLog(string logName)
        {
            if (!System.Diagnostics.EventLog.Exists(logName))
            {
                Logger.Info($"Event Log \"{logName}\" does not exist");
                return new List<MicroLogEntry>();
            }

            var allEventLogs = System.Diagnostics.EventLog.GetEventLogs();

            var eventLog = allEventLogs.First(log => log.LogDisplayName == logName);

            var eventLogEntriesBuffer = new EventLogEntry[eventLog.Entries.Count];

            eventLog.Entries.CopyTo(eventLogEntriesBuffer, 0);

            Logger.Info($"Read {eventLogEntriesBuffer.Length} log entries from Event Log \"{logName}\"");

            return eventLogEntriesBuffer.Select(l => new MicroLogEntry
            {
                Id = Guid.NewGuid().ToString(),
                Source = l.Source,
                Message = l.Message,
                Severity = MicroLogSeverityHelper.MapSeverity(l),
                Timestamp = l.TimeWritten,
                LogName = logName
            }).OrderByDescending(l => l.Timestamp);
        }
    }
}
