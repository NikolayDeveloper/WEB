using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Features.Variance;
using MediatR;
using System.Reflection;
using Autofac.Core;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature;
using Iserv.Niis.ExternalServices.Host.Mediator;
using Iserv.Niis.ExternalServices.Host.Mediator.ExceptionRequestHandler;
using Iserv.Niis.ExternalServices.Host.Mediator.PostRequestHandler;
using Iserv.Niis.ExternalServices.Host.Mediator.PreRequestHandler;
using Module = Autofac.Module;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterSource(new ContravariantRegistrationSource());
            
            builder
                .RegisterType<MediatR.Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder
                .Register<SingleInstanceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t =>
                    {
                        object o;
                        return c.TryResolve(t, out o) ? o : null;
                    };
                })
                .InstancePerLifetimeScope();
            var assembly = typeof(RequisitionSend.Command).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof(IPreRequestHandler<>))))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof(IPostRequestHandler<,>))))
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof(IExceptionRequestHandler<,>))))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                    .Select(interfaceType => new KeyedService("requestHandler", interfaceType)))
                .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(typeof(MediatorPipeline<,>), typeof(IRequestHandler<,>), "requestHandler")
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(LoggingPreRequestHandler<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ValidationPreRequestHandler<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(LoggingPostRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ExceptionRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}