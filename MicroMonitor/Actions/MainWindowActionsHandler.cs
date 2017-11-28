using System;
using System.Threading.Tasks;
using MediatR;
using MicroMonitor.Config;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Actions
{
    class MainWindowActionsHandler : 
        IAsyncNotificationHandler<SetDefaultWindowWidthAndHeight>, 
        IAsyncNotificationHandler<SetDefaultWindowPosition>
    {
        private readonly IAppStore _store;

        public MainWindowActionsHandler(IAppStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task Handle(SetDefaultWindowWidthAndHeight notification)
        {
            var width = AppConfiguration.MainWindowWidth();
            var height = AppConfiguration.MainWindowHeight();

            await _store.Dispatch(new WindowSizeChanged(width, height));
        }

        public async Task Handle(SetDefaultWindowPosition notification)
        {
            var state = _store.GetState();
            var mainWindow = state.MainWindowState.Window;

            var spawnMethod = AppConfiguration.MainWindowSpawnMethod();

            double left;
            double top;

            switch (spawnMethod)
            {
                case WindowSpawnMethod.Cursor:
                    (left, top) = WindowHelper.PositionWindowAtMouseCursor(mainWindow, state.MainWindowState.WindowHeight, state.MainWindowState.WindowWidth);
                    break;
                case WindowSpawnMethod.CenterScreen:
                    (left, top) = WindowHelper.PositionWindowAtCenterScreen(mainWindow);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await _store.Dispatch(new WindowPositionChanged(left, top));
        }
    }
}
