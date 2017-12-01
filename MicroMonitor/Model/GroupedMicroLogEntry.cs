﻿using System.Collections.Immutable;

namespace MicroMonitor.Model
{
    public class GroupedMicroLogEntry
    {
        public string Key { get; set; }
        public ImmutableList<MicroLogEntry> LogEntries { get; set; } = ImmutableList<MicroLogEntry>.Empty;
    }
}
