using System;

namespace MicroMonitor
{
    public class MicroLogEntryMeta
    {
        public bool IsMarkedAsRead { get; set; }
        public DateTime MarkedAsReadTimestamp { get; set; }
        public DateTime ReadFromEventLogTimestamp { get; set; }
    }
}
