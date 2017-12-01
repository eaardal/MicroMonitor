using MediatR;
using MicroMonitor.Actions;

namespace MicroMonitor.Reducers
{
    public class PeekWindowReducer : IPeekWindowReducer, INotificationHandler<OpenedNewPeekWindow>
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
