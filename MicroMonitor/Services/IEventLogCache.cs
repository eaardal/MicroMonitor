using System.Collections.Generic;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    internal interface IEventLogCache
    {
        void InsertOrUpdate(string key, IEnumerable<MicroLogEntry> value);
        IEnumerable<MicroLogEntry> Get(string key);
    }
}