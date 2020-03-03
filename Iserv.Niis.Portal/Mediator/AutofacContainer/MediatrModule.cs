using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Iserv.Niis.Features.Request;
using Iserv.Niis.Portal.Mediator.PreRequestHandlers;
using MediatR;

namespace Iserv.Niis.Portal.Mediator.AutofacContainer
{
    public class MediatrModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediatR.Mediator>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register<SingleInstanceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => c.TryResolve(t, out object o) ? o : null;
                })
                .InstancePerLifetimeScope();

            var assembly = typeof(Create.Command).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof(IAsyncPreRequestHandler<>))))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof(IAsyncPostRequestHandler<,>))))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(IAsyncRequestHandler<,>)))
                    .Select(interfaceType => new KeyedService("asyncRequestHandler", interfaceType)))
                .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(typeof(AsyncMediatorPipeline<,>), typeof(IAsyncRequestHandler<,>), "asyncRequestHandler")
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ValidationPreRequestHandler<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

    }

    public class AsyncMediatorPipeline<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;

        private readonly IAsyncPreRequestHandler<TRequest>[] _preRequestHandlers;

        private readonly IAsyncPostRequestHandler<TRequest, TResponse>[] _postRequestHandlers;

        public AsyncMediatorPipeline(IAsyncRequestHandler<TRequest, TResponse> inner, IAsyncPreRequestHandler<TRequest>[] preRequestHandlers, IAsyncPostRequestHandler<TRequest, TResponse>[] postRequestHandlers)
        {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            foreach (var preRequestHandler in _preRequestHandlers)
            {
                await preRequestHandler.Handle(message);
            }

            var result = await _inner.Handle(message);

            foreach (var postRequestHandler in _postRequestHandlers)
            {
                await postRequestHandler.Handle(message, result);
            }

            return result;
        }
    }
}