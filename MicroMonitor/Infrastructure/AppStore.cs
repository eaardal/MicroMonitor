using MediatR;

namespace MicroMonitor.Infrastructure
{
    interface IAppStore : IStore<AppState>
    {
        
    }
    class AppStore : Store<AppState>
    {
        public AppStore(IMediator mediator, AppState initialState) : base(mediator, initialState)
        {
        }
    }
}
