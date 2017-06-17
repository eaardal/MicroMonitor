using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMonitor.ConsoleApp
{
    class Program
    {
        private const string DefaultDebugEventLog = "MicroMonitor.Debug";
        private const string DefaultDebugEventSource = "MicroMonitor.ConsoleApp";

        static void Main(string[] args)
        {
            Run(args);
            Exit();
        }

        private static void Run(string[] args)
        {
            var input = string.Empty;
            while (input != "exit")
            {
                Console.WriteLine(@"Enter command:");

                input = Console.ReadLine();
                switch (input)
                {
                    case "gen err":
                        CreateErrorEventInEventViewer(DefaultDebugEventLog);
                        break;
                    case "gen inf":
                        CreateInfoEventInEventViewer(DefaultDebugEventLog);
                        break;
                    case "gen warn":
                        CreateWarningEventInEventViewer(DefaultDebugEventLog);
                        break;
                }
            }
        }

        private static void CreateWarningEventInEventViewer(string logName)
        {
            EnsureEventLogExists(logName);

            EventLog.WriteEntry(DefaultDebugEventSource, Lipsum.Text, EventLogEntryType.Warning);

            Console.WriteLine(@"Successfully wrote to event log");
        }

        private static void CreateInfoEventInEventViewer(string logName)
        {
            EnsureEventLogExists(logName);

            EventLog.WriteEntry(DefaultDebugEventSource, Lipsum.Text, EventLogEntryType.Information);

            Console.WriteLine(@"Successfully wrote to event log");
        }

        private static void CreateErrorEventInEventViewer(string logName)
        {
            EnsureEventLogExists(logName);

            EventLog.WriteEntry(DefaultDebugEventSource, Lipsum.Text, EventLogEntryType.Error);

            Console.WriteLine(@"Successfully wrote to event log");
        }

        private static void EnsureEventLogExists(string logName)
        {
            if (!EventLog.SourceExists(logName))
            {
                Console.WriteLine($@"The Event Log {logName} does not exist");
                EventLog.CreateEventSource(DefaultDebugEventSource, logName);
            }
        }

        private static void Exit()
        {
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
