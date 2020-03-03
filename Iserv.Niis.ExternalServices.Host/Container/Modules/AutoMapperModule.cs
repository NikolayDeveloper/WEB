using Autofac;
using AutoMapper;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Mappings;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Mapper.Initialize(cfg => { cfg.AddProfiles(typeof(EgovPayProfile).Assembly); });
            builder.Register(ctx => Mapper.Instance).As<IMapper>();
        }
    }
}