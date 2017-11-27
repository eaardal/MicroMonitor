using System;
using System.Collections.Generic;
using MediatR;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    class RefreshEventLogEntriesStart : INotification { }

    class RefreshEventLogEntriesSuccess : INotification
    {
        public string LogName { get; }
        public IEnumerable<GroupedMicroLogEntry> EventLogEntries { get; }

        public RefreshEventLogEntriesSuccess(string logName, IEnumerable<GroupedMicroLogEntry> eventLogEntries)
        {
            LogName = logName;
            EventLogEntries = eventLogEntries;
        }
    }

    class RefreshEventLogEntriesError : INotification
    {
        public Exception Exception { get; }

        public RefreshEventLogEntriesError(Exception exception)
        {
            Exception = exception;
        }
    }

    class RefreshEventLogEntries : INotification { }

    class StartPollingForEventLogEntries : INotification
    {
        
    }

    class UpdateEventLogEntries : INotification
    {
        public string LogName { get; }
        public IEnumerable<MicroLogEntry> EventLogEntries { get; }

        public UpdateEventLogEntries(string logName, IEnumerable<MicroLogEntry> eventLogEntries)
        {
            LogName = logName;
            EventLogEntries = eventLogEntries;
        }
    }
}