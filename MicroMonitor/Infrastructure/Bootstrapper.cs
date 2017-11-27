using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using CommonServiceLocator;
using MediatR;
using MediatR.Pipeline;

namespace MicroMonitor.Infrastructure
{
    class Bootstrapper
    {
        public static void Wire()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsSelf()
                .AsImplementedInterfaces();

            RegisterMediator(builder);

            var container = builder.Build();

            var autofacServiceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => autofacServiceLocator);
        }

        public static void RegisterMediator(ContainerBuilder builder)
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
