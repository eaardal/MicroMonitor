using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MicroMonitor.Model
{
    public class MicroLogEntry
    {
        public string Source { get; set; }

        public string Message { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MicroLogSeverity Severity { get; set; }

        public DateTime Timestamp { get; set; }

        public string Id { get; set; }

        public string LogName { get; set; }
    }
}