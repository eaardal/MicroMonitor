using MediatR;

namespace MicroMonitor.Infrastructure
{
    public class AppStore : Store<AppState>, IAppStore
    {
        public AppStore(IMediator mediator, AppState initialState) : base(mediator, initialState)
        {
        }
    }
}
