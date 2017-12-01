using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using CommonServiceLocator;
using MediatR;
using MicroMonitor.Reducers;
using MicroMonitor.Services;

namespace MicroMonitor.Infrastructure
{
    public class Bootstrapper
    {
        public static void Wire()
        {
            var builder = new ContainerBuilder();

            var thisAssembly = Assembly.GetExecutingAssembly();

            builder
                .RegisterAssemblyTypes(thisAssembly)
                .Except<CachePoller>()
                .Except<EventLogCache>()
                .Except<EventLogPoller>()
                .Except<IEventLogPollingCoordinator>()
                .Except<AppState>()
                .Except<AppStore>()
                .Except<MainWindowReducer>()
                .Except<DetailsWindowReducer>()
                .Except<PeekWindowReducer>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterType<CachePoller>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EventLogCache>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EventLogPoller>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<IEventLogPollingCoordinator>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AppState>().AsSelf().SingleInstance();
            builder.RegisterType<AppStore>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MainWindowReducer>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DetailsWindowReducer>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PeekWindowReducer>().AsImplementedInterfaces().SingleInstance();

            RegisterMediator(builder);

            var container = builder.Build();

            var autofacServiceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => autofacServiceLocator);
        }

        private static void RegisterMediator(ContainerBuilder builder)
        {
            // https://github.com/jbogard/MediatR/wiki

            // enables contravariant Resolve() for interfaces with single contravariant ("in") arg
            builder
                .RegisterSource(new ContravariantRegistrationSource());

            // mediator itself
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // request handlers
            builder
                .Register<SingleInstanceFactory>(ctx => {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => { object o; return c.TryResolve(t, out o) ? o : null; };
                })
                .InstancePerLifetimeScope();

            // notification handlers
            builder
                .Register<MultiInstanceFactory>(ctx => {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                })
                .InstancePerLifetimeScope();

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            //builder.RegisterAssemblyTypes(typeof(MyType).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
            //builder.RegisterType<MyHandler>().AsImplementedInterfaces().InstancePerDependency(); 
        }
    }
}
