using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                .Select(grp => new GroupedMicroLogEntry
                {
                    Key = grp.Key.Date == DateTime.Today ? "Today" : grp.Key.ToString("dd.MM.yy"),
                    LogEntries = new ObservableCollection<MicroLogEntry>(grp.Select(e => e).OrderByDescending(e => e.Timestamp))
                });
        }
    }
}
