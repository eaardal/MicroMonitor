using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMonitor
{
    class MicroLogRepository
    {
        private MicroLogPersistedRegistry _persistedRegistry = new MicroLogPersistedRegistry();

        public void Save(MicroLogEntry entry)
        {
            Save(new[] { entry });
        }

        public void Save(IEnumerable<MicroLogEntry> logEntries)
        {
            _persistedRegistry.Save(logEntries);
        }

        public IEnumerable<MicroLogEntry> Read()
        {
            var entriesRaw = _persistedRegistry.Read();
            return null;
        }
    }
}
