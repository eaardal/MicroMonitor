using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    public class PeekWindowReducer : IPeekWindowReducer, INotificationHandler<OpenedNewPeekWindow>
    {
        private readonly PeekWindowState _state;

        public PeekWindowReducer(IAppStore store)
        {
            _state = store.GetState().PeekWindowState;
        }

        public Task Handle(OpenedNewPeekWindow message, CancellationToken cancellationToken)
        {
            _state.PeekWindow = message.NewPeekWindow;
            _state.PeekWindowId = message.NewPeekWindowId;
            
            return Task.CompletedTask;
        }
    }
}
