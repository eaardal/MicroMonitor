using System;

namespace MicroMonitor
{
    public class MicroLogEntry
    {
        public MicroLogEntryMeta Meta { get; set; } = new MicroLogEntryMeta();
        public string Source { get; set; }
        public string Message { get; set; }
        public MicroLogSeverity Severity { get; set; }

        // Ugly af hack to get more than one parameter into the value converter for now...
        public string SeverityHack => $"{Severity};{Meta.IsMarkedAsRead}";
        public DateTime Timestamp { get; set; }
        public string Id { get; set; }

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