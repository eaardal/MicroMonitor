using System.Collections.Generic;
using System.Threading.Tasks;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    internal interface IEventLogService
    {
        Task<IEnumerable<GroupedMicroLogEntry>> GetEventLogEntries(string logName);
    }
}