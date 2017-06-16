using System.Configuration;

namespace MicroMonitor
{
    class AppConfiguration
    {
        public static string LogName()
        {
            return ConfigurationManager.AppSettings["LogName"];
        }

        public static int PollIntervalSeconds()
        {
            return int.Parse(ConfigurationManager.AppSettings["PollIntervalSeconds"]);
        }
    }
}
