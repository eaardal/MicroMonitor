using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

        public string LongSummary => $"{Timestamp:dd.MM.yy HH:mm:ss} {LogName}\\{Source}\n\n {Message}";

        public string ShortSummary => $"{Timestamp:dd.MM.yy HH:mm:ss} {LogName}\\{Source}";
    }
}