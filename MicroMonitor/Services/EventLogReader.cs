using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    class EventLogReader : IEventLogReader
    {
        public IEnumerable<MicroLogEntry> ReadEventLog(string logName)
        {
            if (!EventLog.Exists(logName))
            {
                Logger.Error($"Event Log \"{logName}\" does not exist");
                return new List<MicroLogEntry>();
            }

            var allEventLogs = EventLog.GetEventLogs();

            var eventLog = allEventLogs.First(log => log.LogDisplayName == logName);

            var eventLogEntriesBuffer = new EventLogEntry[eventLog.Entries.Count];

            eventLog.Entries.CopyTo(eventLogEntriesBuffer, 0);

            Logger.Verbose($"Read {eventLogEntriesBuffer.Length} log entries from Event Log \"{logName}\"");

            return eventLogEntriesBuffer.Select(l => new MicroLogEntry
            {
                Id = $"{logName}_{l.Source}_{l.EntryType}_{l.TimeWritten.Ticks}",
                Source = l.Source,
                Message = l.Message,
                Severity = MicroLogSeverityHelper.MapSeverity(l),
                Timestamp = l.TimeWritten,
                LogName = logName

            }).OrderByDescending(l => l.Timestamp);
        }

        public async Task<IEnumerable<MicroLogEntry>> ReadEventLogAsync(string logName)
        {
            return await Task.Run(() => ReadEventLog(logName));
        }
    }
}
