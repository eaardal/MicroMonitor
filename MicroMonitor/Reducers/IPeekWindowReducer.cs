using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    public interface IPeekWindowReducer : IReducer
    {
        void Handle(OpenedNewPeekWindow message);
    }
}