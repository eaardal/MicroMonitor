using System.Threading;
using System.Threading.Tasks;
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
        
        public Task Handle(CreatedNewDetailsWindow message, CancellationToken cancellationToken)
        {
            _state.OpenDetailsWindows.Add(message.NewDetailsWindow);

            return Task.CompletedTask;
        }

        public Task Handle(CloseAllOpenDetailsWindows notification, CancellationToken cancellationToken)
        {
            foreach (var openDetailWindow in _state.OpenDetailsWindows)
            {
                openDetailWindow.Close();
            }

            _state.OpenDetailsWindows.Clear();

            return Task.CompletedTask;
        }
    }
}
