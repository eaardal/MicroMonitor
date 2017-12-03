using System;
using MicroMonitor.Config;
using Serilog;

namespace MicroMonitor.Infrastructure
{
    public class Logger
    {
        private static ILogger _serilog;
        
        public static void Create(IAppConfiguration configuration)
        {
            var config = new LoggerConfiguration();
            config.WriteTo.RollingFile("Logs\\{Date}.txt").WriteTo.Seq("http://localhost:5341");
            config.MinimumLevel.Is(configuration.LogLevel());

            _serilog = config.CreateLogger();
        }

        public static void Verbose(string message, params object[] args)
        {
            _serilog.Verbose(message, args);
        }

        public static void Debug(string message, params object[] args)
        {
            _serilog.Debug(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            _serilog.Information(message, args);
        }
        
        public static void Warning(string message, params object[] args)
        {
            _serilog.Warning(message, args);
        }

        public static void Error(Exception ex, string message, params object[] args)
        {
            _serilog.Error(ex, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            _serilog.Error(message, args);
        }
    }
}
