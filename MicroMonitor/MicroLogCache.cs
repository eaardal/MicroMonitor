using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MicroMonitor
{
    class MicroLogCache
    {
        private readonly ConcurrentDictionary<string, IEnumerable<MicroLogEntry>> _cache = new ConcurrentDictionary<string, IEnumerable<MicroLogEntry>>();
        private static MicroLogCache _instance;

        public void InsertOrUpdate(string key, IEnumerable<MicroLogEntry> value)
        {
            var valueArray = value.ToArray();

            _cache.AddOrUpdate(key, valueArray, (existingKey, existingValue) => valueArray);
        }

        public IEnumerable<MicroLogEntry> Get(string key)
        {
            return _cache.ContainsKey(key) ? _cache[key] : new List<MicroLogEntry>();
        }

        public static MicroLogCache Instance => _instance ?? (_instance = new MicroLogCache());
    }
}
