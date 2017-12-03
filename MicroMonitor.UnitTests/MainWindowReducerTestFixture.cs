using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MicroMonitor.Config;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Reducers;
using MicroMonitor.Utilities;
using Moq;
using Serilog.Events;

namespace MicroMonitor.UnitTests
{
    internal class MainWindowReducerTestFixture : ITestFixture<MainWindowReducer>
    {
        public AppState State { get; } = new AppState();
        public Mock<IAppStore> Store { get; } = new Mock<IAppStore>();
        public Mock<IAppConfiguration> Configuration { get; } = new Mock<IAppConfiguration>();

        public MainWindowReducerTestFixture()
        {
            Configuration
                .Setup(x => x.LogLevel())
                .Returns(LogEventLevel.Debug);

            Logger.Create(Configuration.Object);

            Store
                .Setup(x => x.GetState())
                .Returns(State);
        }

        public MainWindowReducer CreateSut()
        {
            return new MainWindowReducer(Store.Object);
        }

        public MainWindowReducerTestFixture SetupLogEntries(List<MicroLogEntry> logEntries)
        {
            State.MainWindowState.LogEntries = new ObservableCollection<MicroLogEntry>(logEntries.OrderByDescending(e => e.Timestamp));
            return this;
        }

        public MainWindowReducerTestFixture SetupGroupedLogEntries(List<MicroLogEntry> logEntries)
        {
            State.MainWindowState.GroupedLogEntries =
                new ObservableCollection<GroupedMicroLogEntry>(LogEntryUtils.GroupLogEntriesByDate(logEntries));

            return this;
        }
    }
}