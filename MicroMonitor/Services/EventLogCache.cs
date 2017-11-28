using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MicroMonitor.Model;

namespace MicroMonitor.Services
{
    class EventLogCache : IEventLogCache
    {
        private readonly ConcurrentDictionary<string, IEnumerable<MicroLogEntry>> _cache = new ConcurrentDictionary<string, IEnumerable<MicroLogEntry>>();

        public void InsertOrUpdate(string key, IEnumerable<MicroLogEntry> value)
        {
            var valueArray = value.ToArray();

            _cache.AddOrUpdate(key, valueArray, (existingKey, existingValue) => valueArray);
        }

        public IEnumerable<MicroLogEntry> Get(string key)
        {
            return _cache.ContainsKey(key) ? _cache[key] : new List<MicroLogEntry>();
        }
    }
}
