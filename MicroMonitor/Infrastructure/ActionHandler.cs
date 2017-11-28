namespace MicroMonitor.Infrastructure
{
    abstract class ActionHandler<TStore, TState> where TStore : IStore<TState>
    {
        protected TStore Store { get; }

        protected ActionHandler(TStore store)
        {
            Store = store;
        }
    }
}
