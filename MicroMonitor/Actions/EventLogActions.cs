using System;
using System.Collections.Generic;
using MediatR;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    public class RefreshEventLogEntriesStart : INotification { }

    public class RefreshEventLogEntriesSuccess : INotification
    {
        public string LogName { get; }
        public IEnumerable<GroupedMicroLogEntry> EventLogEntries { get; }

        public RefreshEventLogEntriesSuccess(string logName, IEnumerable<GroupedMicroLogEntry> eventLogEntries)
        {
            LogName = logName;
            EventLogEntries = eventLogEntries;
        }
    }

    public class RefreshEventLogEntriesError : INotification
    {
        public Exception Exception { get; }

        public RefreshEventLogEntriesError(Exception exception)
        {
            Exception = exception;
        }
    }

    public class RefreshEventLogEntries : INotification { }

    public class StartPollingForEventLogEntries : INotification
    {
        
    }

    public class UpdateEventLogEntries : INotification
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