using System;
using System.Collections.Generic;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    internal interface ICachePoller
    {
        void ReadOnInterval(string logName, int seconds, Action<IEnumerable<MicroLogEntry>> onRetrieved);
    }
}