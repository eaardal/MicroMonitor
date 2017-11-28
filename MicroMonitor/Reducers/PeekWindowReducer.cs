using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    class PeekWindowReducer : IReducer, INotificationHandler<OpenedNewPeekWindow>
    {
        private readonly PeekWindowState _state;

        public PeekWindowReducer()
        {
            _state = new PeekWindowState();
        }

        public void Handle(OpenedNewPeekWindow message)
        {
            _state.PeekWindow = message.NewPeekWindow;
            _state.PeekWindowId = message.NewPeekWindowId;
        }
    }
}
