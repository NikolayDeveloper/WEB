using Autofac;
using AutoMapper;
using Iserv.Niis.ExternalServices.Features.Mapping.System;

namespace Iserv.Niis.ExternalServices.StatusSender.Host.Container.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Mapper.Initialize(cfg => { cfg.AddProfiles(typeof(LogSystemInfoProfile).Assembly); });
        }
    }
}