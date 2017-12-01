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

        public void Handle(OpenedNewPeekWindow message)
        {
            _state.PeekWindow = message.NewPeekWindow;
            _state.PeekWindowId = message.NewPeekWindowId;
        }
    }
}
