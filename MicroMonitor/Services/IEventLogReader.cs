using System.Collections.Generic;
using System.Threading.Tasks;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    internal interface IEventLogReader
    {
        IEnumerable<MicroLogEntry> ReadEventLog(string logName);
        Task<IEnumerable<MicroLogEntry>> ReadEventLogAsync(string logName);
    }
}