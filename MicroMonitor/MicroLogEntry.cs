using System;

namespace MicroMonitor
{
    public class MicroLogEntry
    {
        public string Source { get; set; }
        public string Message { get; set; }
        public MicroLogSeverity Severity { get; set; }
        public string SeverityText => Severity.ToString();
        public DateTime Timestamp { get; set; }

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