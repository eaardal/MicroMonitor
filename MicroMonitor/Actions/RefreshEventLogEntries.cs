using System;
using System.Collections.Generic;
using MediatR;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    class RefreshEventLogEntriesStart : IRequest { }

    class RefreshEventLogEntriesSuccess : IRequest
    {
        public string LogName { get; }
        public IEnumerable<GroupedMicroLogEntry> EventLogEntries { get; }

        public RefreshEventLogEntriesSuccess(string logName, IEnumerable<GroupedMicroLogEntry> eventLogEntries)
        {
            LogName = logName;
            EventLogEntries = eventLogEntries;
        }
    }

    class RefreshEventLogEntriesError : IRequest
    {
        public Exception Exception { get; }

        public RefreshEventLogEntriesError(Exception exception)
        {
            Exception = exception;
        }
    }

    class RefreshEventLogEntries : IRequest { }
}