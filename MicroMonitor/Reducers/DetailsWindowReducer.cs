using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    public class DetailsWindowReducer : IDetailsWindowReducer, 
        INotificationHandler<CreatedNewDetailsWindow>,
        INotificationHandler<CloseAllOpenDetailsWindows>
    {
        private readonly DetailsWindowState _state;

        public DetailsWindowReducer(IAppStore store)
        {
            _state = store.GetState().DetailsWindowState;
        }
        
        public void Handle(CreatedNewDetailsWindow message)
        {
            _state.OpenDetailsWindows.Add(message.NewDetailsWindow);
        }

        public void Handle(CloseAllOpenDetailsWindows notification)
        {
            foreach (var openDetailWindow in _state.OpenDetailsWindows)
            {
                openDetailWindow.Close();
            }

            _state.OpenDetailsWindows.Clear();
        }
    }
}
