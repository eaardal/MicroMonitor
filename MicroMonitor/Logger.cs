using System;

namespace MicroMonitor
{
    public class Logger
    {
        public static void Info(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }

        public static void Error(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }
    }
}
