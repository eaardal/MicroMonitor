using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MicroMonitor.Model;

namespace MicroMonitor.Utilities
{
    class LogEntryUtils
    {
        public static IEnumerable<GroupedMicroLogEntry> GroupLogEntriesByDate(IEnumerable<MicroLogEntry> logEntries)
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
