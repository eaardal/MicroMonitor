using System;
using System.Collections.Generic;
using MicroMonitor.Model;

namespace MicroMonitor.Services.MicroLog
{
    internal interface IMicroLogReader
    {
        void ReadOnInterval(string logName, int seconds, Action<IEnumerable<MicroLogEntry>> onRetrieved);
    }
}