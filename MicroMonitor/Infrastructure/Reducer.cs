namespace MicroMonitor.Infrastructure
{
    abstract class Reducer<TStore, TState> : IReducer where TStore : IStore<TState>
    {
        protected TStore Store { get; }

        protected Reducer(TStore store)
        {
            Store = store;
        }
    }
}
