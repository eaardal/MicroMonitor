using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    public interface IDetailsWindowReducer : IReducer
    {
        void Handle(CreatedNewDetailsWindow message);
        void Handle(CloseAllOpenDetailsWindows notification);
    }
}