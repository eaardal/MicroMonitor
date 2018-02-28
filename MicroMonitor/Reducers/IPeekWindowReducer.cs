using System.Threading;
using System.Threading.Tasks;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    public interface IPeekWindowReducer : IReducer
    {
        Task Handle(OpenedNewPeekWindow message, CancellationToken cancellationToken);
    }
}