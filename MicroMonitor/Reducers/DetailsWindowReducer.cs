using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    class DetailsWindowReducer : IReducer, INotificationHandler<CreatedNewDetailsWindow>
    {
        private readonly DetailsWindowState _state;

        public DetailsWindowReducer()
        {
            _state = new DetailsWindowState();
        }
        
        public void Handle(CreatedNewDetailsWindow message)
        {
            _state.OpenDetailsWindows.Add(message.NewDetailsWindow);
        }
    }
}
