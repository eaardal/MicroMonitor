using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMonitor
{
    class MicroLogPersistedRegistry
    {
        private const string MicroMonitorDirectoryName = "MicroMonitor";
        private const string RegistryFileName = "registry";

        public void Save(MicroLogEntry entry)
        {
            Save(new[] {entry});
        }

        public void Save(IEnumerable<MicroLogEntry> logEntries)
        {
            EnsureDirectoryExists();

            var persistedEntries = Read();

            var newEntries = logEntries.Where(e => !persistedEntries.Contains(e.Id)).Select(e => e.Id);

            var registryFilePath = GetRegistryFilePath();

            File.AppendAllLines(registryFilePath, newEntries);
        }

        public string[] Read()
        {
            EnsureDirectoryExists();

            var registryFilePath = GetRegistryFilePath();
            
            return File.ReadAllLines(registryFilePath);
        }

        private void EnsureDirectoryExists()
        {
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var fullRegistryDirectoryPath = Path.Combine(programData, MicroMonitorDirectoryName);

            if (!Directory.Exists(fullRegistryDirectoryPath))
            {
                Directory.CreateDirectory(fullRegistryDirectoryPath);
            }

            var registryFilePath = Path.Combine(fullRegistryDirectoryPath, RegistryFileName);

            if (!File.Exists(registryFilePath))
            {
                File.Create(registryFilePath);
            }
        }

        private string GetRegistryFilePath()
        {
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            
            return Path.Combine(programData, MicroMonitorDirectoryName, RegistryFileName);
        }
    }
}
