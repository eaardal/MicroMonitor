using System;
using System.Collections.Generic;
using System.Linq;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Reducers;
using NUnit.Framework;
using Shouldly;

namespace MicroMonitor.UnitTests
{
    [TestFixture]
    public class MainWindowReducerTests
    {
        private const string LogName = "MainWindowReducerTests";

        private MainWindowReducerTestFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new MainWindowReducerTestFixture();
        }

        [Test]
        public void CreatesSystemUnderTest()
        {
            var sut = _fixture.CreateSut();

            sut.ShouldNotBeNull();
            sut.ShouldBeAssignableTo<IMainWindowReducer>();
            sut.ShouldBeAssignableTo<IReducer>();
        }

        [Test]
        public void UpdateEventLogEntries_WhenNoExistingLogEntries_InsertsLogEntries()
        {
            var logEntries = new List<MicroLogEntry>
            {
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(1)).Build(),
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(2)).Build(),
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(3)).Build()
            };

            var sut = _fixture.CreateSut();

            var action = new UpdateEventLogEntries(LogName, logEntries);

            sut.Handle(action);

            var state = _fixture.State.MainWindowState;

            state.LogEntries.Count.ShouldBe(3);
            state.LogEntries.ElementAt(0).Id.ShouldBe(logEntries.ElementAt(2).Id);
            state.LogEntries.ElementAt(1).Id.ShouldBe(logEntries.ElementAt(1).Id);
            state.LogEntries.ElementAt(2).Id.ShouldBe(logEntries.ElementAt(0).Id);

            state.GroupedLogEntries.Count.ShouldBe(1);
            var groupedLogEntries = state.GroupedLogEntries.Single().LogEntries;
            groupedLogEntries.Count.ShouldBe(3);
            groupedLogEntries.ElementAt(0).Id.ShouldBe(logEntries.ElementAt(2).Id);
            groupedLogEntries.ElementAt(1).Id.ShouldBe(logEntries.ElementAt(1).Id);
            groupedLogEntries.ElementAt(2).Id.ShouldBe(logEntries.ElementAt(0).Id);
        }
        
        [Test]
        public void UpdateEventLogEntries_WhenExistingLogEntries_InsertsNewLogEntriesAtTopOfTheList()
        {
            var existingLogEntries = new List<MicroLogEntry>
            {
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(3)).Build(),
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(1)).Build(),
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(2)).Build()
            };

            var newLogEntries = new List<MicroLogEntry>
            {
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(5)).Build(),
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(6)).Build(),
                new MicroLogEntryBuilder().WithTimestamp(DateTime.Now.AddSeconds(4)).Build()
            };

            var sut = _fixture
                .SetupLogEntries(existingLogEntries)
                .SetupGroupedLogEntries(existingLogEntries)
                .CreateSut();
            
            var action = new UpdateEventLogEntries(LogName, newLogEntries);

            sut.Handle(action);

            var state = _fixture.State.MainWindowState;

            state.LogEntries.Count.ShouldBe(6);
            state.LogEntries.ElementAt(0).Id.ShouldBe(newLogEntries.ElementAt(1).Id);
            state.LogEntries.ElementAt(1).Id.ShouldBe(newLogEntries.ElementAt(0).Id);
            state.LogEntries.ElementAt(2).Id.ShouldBe(newLogEntries.ElementAt(2).Id);
            state.LogEntries.ElementAt(3).Id.ShouldBe(existingLogEntries.ElementAt(0).Id);
            state.LogEntries.ElementAt(4).Id.ShouldBe(existingLogEntries.ElementAt(2).Id);
            state.LogEntries.ElementAt(5).Id.ShouldBe(existingLogEntries.ElementAt(1).Id);

            state.GroupedLogEntries.Count.ShouldBe(1);
            var groupedLogEntries = state.GroupedLogEntries.Single().LogEntries;
            groupedLogEntries.Count.ShouldBe(6);
            groupedLogEntries.ElementAt(0).Id.ShouldBe(newLogEntries.ElementAt(1).Id);
            groupedLogEntries.ElementAt(1).Id.ShouldBe(newLogEntries.ElementAt(0).Id);
            groupedLogEntries.ElementAt(2).Id.ShouldBe(newLogEntries.ElementAt(2).Id);
            groupedLogEntries.ElementAt(3).Id.ShouldBe(existingLogEntries.ElementAt(0).Id);
            groupedLogEntries.ElementAt(4).Id.ShouldBe(existingLogEntries.ElementAt(2).Id);
            groupedLogEntries.ElementAt(5).Id.ShouldBe(existingLogEntries.ElementAt(1).Id);
        }
    }
}
