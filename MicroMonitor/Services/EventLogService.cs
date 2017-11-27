using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroMonitor.Model;
using MicroMonitor.Services.EventLog;
using MicroMonitor.Utilities;

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

            return LogEntryUtils.GroupLogEntriesByDate(logEntries);
        }
    }
}
