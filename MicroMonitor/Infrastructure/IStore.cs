using System.Threading.Tasks;
using MediatR;

namespace MicroMonitor.Infrastructure
{
    public interface IStore<out TState>
    {
        TState GetState();
        Task Dispatch<TMessage>(TMessage message) where TMessage : INotification;
    }
}