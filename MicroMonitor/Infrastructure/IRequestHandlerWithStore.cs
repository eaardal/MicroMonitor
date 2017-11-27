namespace MicroMonitor.Infrastructure
{
    internal interface IRequestHandlerWithStore<in TMessage, in TState>
    {
        void Handle(TMessage message, TState state);
    }
}