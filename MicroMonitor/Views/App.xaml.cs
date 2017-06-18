using MicroMonitor.Interop;

namespace MicroMonitor.Views
{
    public partial class App
    {
        public App()
        {
#if DEBUG
            Startup += (sender, args) =>
            {
                ConsoleWindow.Create();
            };

            Exit += (sender, args) =>
            {
                ConsoleWindow.Destroy();
            };
#endif
        }

    }
}
