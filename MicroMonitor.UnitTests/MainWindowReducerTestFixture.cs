﻿using System.Collections.Generic;
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
        public Mock<IConfiguration> Configuration { get; } = new Mock<IConfiguration>();

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
            State.MainWindowState.LogEntries.Clear();

            foreach (var microLogEntry in logEntries.OrderByDescending(e => e.Timestamp))
            {
                State.MainWindowState.LogEntries.Add(microLogEntry);
            }
            
            return this;
        }

        public MainWindowReducerTestFixture SetupGroupedLogEntries(List<MicroLogEntry> logEntries)
        {
            State.MainWindowState.GroupedLogEntries.Clear();

            foreach (var microLogEntry in LogEntryUtils.GroupLogEntriesByDate(logEntries))
            {
                State.MainWindowState.GroupedLogEntries.Add(microLogEntry);
            }

            return this;
        }
    }
}