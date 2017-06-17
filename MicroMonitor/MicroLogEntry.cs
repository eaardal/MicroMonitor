using System;

namespace MicroMonitor
{
    public class MicroLogEntry
    {
        public MicroLogEntryMeta Meta { get; set; } = new MicroLogEntryMeta();
        public string Source { get; set; }
        public string Message { get; set; }
        public MicroLogSeverity Severity { get; set; }
        public DateTime Timestamp { get; set; }
        public string Id { get; set; }
        public string LogName { get; set; }

        public MicroLogEntry()
        {

        }

        public MicroLogEntry(string source, string message, DateTime timestamp, MicroLogSeverity severity)
        {
            Source = source;
            Message = message;
            Timestamp = timestamp;
            Severity = severity;
        }
    }
}