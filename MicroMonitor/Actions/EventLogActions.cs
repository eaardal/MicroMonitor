using System;
using System.Collections.Generic;
using MicroMonitor.Model;
using Action = MicroMonitor.Infrastructure.Action;

namespace MicroMonitor.Actions
{
    public class RefreshEventLogEntriesStart : Action
    {
        public override string ToString()
        {
            return "REFRESH_EVENT_LOG_ENTRIES/START";
        }
    }

    public class RefreshEventLogEntriesSuccess : Action
    {
        public string LogName { get; }
        public IEnumerable<MicroLogEntry> EventLogEntries { get; }

        public RefreshEventLogEntriesSuccess(string logName, IEnumerable<MicroLogEntry> eventLogEntries)
        {
            LogName = logName;
            EventLogEntries = eventLogEntries;
        }

        public override string ToString()
        {
            return "REFRESH_EVENT_LOG_ENTRIES/SUCCESS";
        }
    }

    public class RefreshEventLogEntriesError : Action
    {
        public Exception Exception { get; }

        public RefreshEventLogEntriesError(Exception exception)
        {
            Exception = exception;
        }

        public override string ToString()
        {
            return "REFRESH_EVENT_LOG_ENTRIES/ERROR";
        }
    }

    public class RefreshEventLogEntries : Action
    {
        public override string ToString()
        {
            return "REFRESH_EVENT_LOG_ENTRIES";
        }
    }

    public class StartPollingForEventLogEntries : Action
    {
        public override string ToString()
        {
            return "START_POLLING_FOR_EVENT_LOG_ENTRIES";
        }
    }

    public class UpdateEventLogEntries : Action
    {
        public string LogName { get; }
        public IEnumerable<MicroLogEntry> EventLogEntries { get; }

        public UpdateEventLogEntries(string logName, IEnumerable<MicroLogEntry> eventLogEntries)
        {
            LogName = logName;
            EventLogEntries = eventLogEntries;
        }

        public override string ToString()
        {
            return "UPDATE_EVENT_LOG_ENTRIES";
        }
    }
}