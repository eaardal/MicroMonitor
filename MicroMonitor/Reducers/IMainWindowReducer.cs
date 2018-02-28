using System.Threading;
using System.Threading.Tasks;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    public interface IMainWindowReducer : IReducer
    {
        Task Handle(RefreshEventLogEntriesStart message, CancellationToken cancellationToken);
        Task Handle(RefreshEventLogEntriesSuccess message, CancellationToken cancellationToken);
        Task Handle(RefreshEventLogEntriesError message, CancellationToken cancellationToken);
        Task Handle(ToggleHeaderPanelVisibility message, CancellationToken cancellationToken);
        Task Handle(CreatedNewDetailsWindow message, CancellationToken cancellationToken);
        Task Handle(SetTraversingIndex message, CancellationToken cancellationToken);
        Task Handle(MainWindowActivated message, CancellationToken cancellationToken);
        Task Handle(WindowPositionChanged message, CancellationToken cancellationToken);
        Task Handle(WindowSizeChanged message, CancellationToken cancellationToken);
        Task Handle(SetMainWindow message, CancellationToken cancellationToken);
        Task Handle(SetLastReadText message, CancellationToken cancellationToken);
        Task Handle(UpdateEventLogEntries message, CancellationToken cancellationToken);
        Task Handle(CloseAllOpenDetailsWindows message, CancellationToken cancellationToken);
        Task Handle(MouseEnterLogEntryBoundaries message, CancellationToken cancellationToken);
        Task Handle(MouseLeaveLogEntryBoundaries message, CancellationToken cancellationToken);
    }
}