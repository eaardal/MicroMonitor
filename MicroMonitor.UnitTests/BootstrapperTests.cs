using System;
using CommonServiceLocator;
using MicroMonitor.Infrastructure;
using MicroMonitor.Reducers;
using MicroMonitor.Services;
using NUnit.Framework;
using Shouldly;

namespace MicroMonitor.UnitTests
{
    [TestFixture]
    public class BootstrapperTests
    {
        [TestCase(typeof(ICachePoller))]
        [TestCase(typeof(IEventLogCache))]
        [TestCase(typeof(IEventLogPoller))]
        [TestCase(typeof(IEventLogPollingCoordinator))]
        [TestCase(typeof(AppState))]
        [TestCase(typeof(IAppStore))]
        [TestCase(typeof(IStore<AppState>))]
        [TestCase(typeof(Store<AppState>))]
        [TestCase(typeof(IMainWindowReducer))]
        [TestCase(typeof(IDetailsWindowReducer))]
        [TestCase(typeof(IPeekWindowReducer))]
        public void RegistersServicesAsSingleton(Type service)
        {
            Bootstrapper.Wire();
            var ioc = ServiceLocator.Current;

            var instance1 = ioc.GetInstance(service);
            var instance2 = ioc.GetInstance(service);
            var instance3 = ioc.GetInstance(service);

            instance1.ShouldBeSameAs(instance2);
            instance2.ShouldBeSameAs(instance3);
        }
    }
}
