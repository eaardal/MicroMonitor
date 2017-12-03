using System;
using MicroMonitor.Model;

namespace MicroMonitor.UnitTests
{
    class MicroLogEntryBuilder : ITestDataBuilder<MicroLogEntry>
    {
        private string _id = Guid.NewGuid().ToString();
        private DateTime _timestamp = DateTime.Now;
        private string _logName = "DefaultLogName";
        private string _message = string.Empty;
        private MicroLogSeverity _severity = MicroLogSeverity.Info;
        private string _source = "DefaultSource";

        public MicroLogEntry Build()
        {
            return new MicroLogEntry
            {
                Id = _id,
                Timestamp = _timestamp,
                LogName = _logName,
                Message = _message,
                Severity = _severity,
                Source = _source
            };
        }

        public MicroLogEntryBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public MicroLogEntryBuilder WithTimestamp(DateTime timestamp)
        {
            _timestamp = timestamp;
            return this;
        }

        public MicroLogEntryBuilder WithLogName(string logName)
        {
            _logName = logName;
            return this;
        }

        public MicroLogEntryBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public MicroLogEntryBuilder WithSeverity(MicroLogSeverity severity)
        {
            _severity = severity;
            return this;
        }

        public MicroLogEntryBuilder WithSource(string source)
        {
            _source = source;
            return this;
        }
    }
}
