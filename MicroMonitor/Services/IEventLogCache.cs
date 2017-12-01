using System.Collections.Generic;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    public interface IEventLogCache
    {
        void InsertOrUpdate(string key, IEnumerable<MicroLogEntry> value);
        IEnumerable<MicroLogEntry> Get(string key);
    }
}