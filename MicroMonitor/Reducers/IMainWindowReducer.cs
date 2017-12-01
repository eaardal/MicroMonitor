using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Reducers
{
    public interface IMainWindowReducer : IReducer
    {
        void Handle(RefreshEventLogEntriesStart message);
        void Handle(RefreshEventLogEntriesSuccess message);
        void Handle(RefreshEventLogEntriesError message);
        void Handle(ToggleHeaderPanelVisibility message);
        void Handle(CreatedNewDetailsWindow message);
        void Handle(SetTraversingIndex message);
        void Handle(MainWindowActivated message);
        void Handle(WindowPositionChanged message);
        void Handle(WindowSizeChanged message);
        void Handle(SetMainWindow message);
        void Handle(SetLastReadText message);
        void Handle(UpdateEventLogEntries message);
        void Handle(CloseAllOpenDetailsWindows message);
        void Handle(MouseEnterLogEntryBoundaries message);
        void Handle(MouseLeaveLogEntryBoundaries message);
    }
}