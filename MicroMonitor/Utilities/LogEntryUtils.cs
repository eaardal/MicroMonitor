using System;
using System.Collections.Generic;
using System.Linq;
using MicroMonitor.Model;

namespace MicroMonitor.Utilities
{
    public class LogEntryUtils
    {
        public static IEnumerable<GroupedMicroLogEntry> GroupLogEntriesByDate(IEnumerable<MicroLogEntry> logEntries)
        {
            return logEntries
                .GroupBy(e => e.Timestamp.Date)
                .Select(grp =>
                {
                    var entry = new GroupedMicroLogEntry
                    {
                        Key = grp.Key.Date == DateTime.Today ? "Today" : grp.Key.ToString("dd.MM.yy")
                    };

                    var entries = grp.Select(e => e)
                        .OrderByDescending(e => e.Timestamp);

                    foreach (var e in entries)
                    {
                        entry.LogEntries.Add(e);
                    }

                    return entry;
                });
        }
    }
}
