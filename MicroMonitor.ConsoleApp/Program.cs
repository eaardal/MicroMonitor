using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMonitor.ConsoleApp
{
    class Program
    {
        private const string DefaultDebugEventLog = "MicroMonitor-Debug";

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
            throw new NotImplementedException();
        }

        private static void CreateInfoEventInEventViewer(string logName)
        {
            throw new NotImplementedException();
        }

        private static void CreateErrorEventInEventViewer(string logName)
        {
            throw new NotImplementedException();
        }

        private static void Exit()
        {
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
