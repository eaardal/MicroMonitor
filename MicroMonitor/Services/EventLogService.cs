using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using MicroMonitor.Engine.EventLog;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    class EventLogService : IEventLogService
    {
        private readonly EventLogReader _eventLogReader;

        public EventLogService(EventLogReader eventLogReader)
        {
            _eventLogReader = eventLogReader ?? throw new ArgumentNullException(nameof(eventLogReader));
        }
        
        public async Task<IEnumerable<GroupedMicroLogEntry>> GetEventLogEntries(string logName)
        {
            var logEntries = await _eventLogReader.ReadEventLogAsync(logName);

            return GroupLogEntriesByDate(logEntries);
        }

        private static IEnumerable<GroupedMicroLogEntry> GroupLogEntriesByDate(IEnumerable<MicroLogEntry> logEntries)
        {
            return logEntries
                .GroupBy(e => e.Timestamp.Date)
                .Select(grp => new GroupedMicroLogEntry
                {
                    Key = grp.Key.Date == DateTime.Today ? "Today" : grp.Key.ToString("dd.MM.yy"),
                    LogEntries = grp.Select(e => e).ToImmutableList()
                });
        }
    }
}
