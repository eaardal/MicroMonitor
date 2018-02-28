using System.Threading.Tasks;

namespace MicroMonitor.Infrastructure
{
    public interface IStore<out TState>
    {
        TState GetState();
        TState State { get; }
        Task Dispatch<TMessage>(TMessage message) where TMessage : Action;
    }
}