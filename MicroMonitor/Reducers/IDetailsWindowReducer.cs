using System.Threading;
using System.Threading.Tasks;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    public interface IDetailsWindowReducer : IReducer
    {
        Task Handle(CreatedNewDetailsWindow message, CancellationToken cancellationToken);
        Task Handle(CloseAllOpenDetailsWindows notification, CancellationToken cancellationToken);
    }
}