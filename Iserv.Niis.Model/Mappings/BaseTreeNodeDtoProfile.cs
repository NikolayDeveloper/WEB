using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataBridge.Repositories;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Model.BusinessLogic;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Model.Mappings
{
    public class BaseTreeNodeDtoProfile : Profile
    {
        public BaseTreeNodeDtoProfile()
        {
            CreateMap<DicICFEM, BaseTreeNodeDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label,
                    opt => opt.MapFrom(src => $"{src.Code} - {src.NameRu ?? string.Empty}"))
                .ForMember(dest => dest.Selectable, opt => opt.UseValue(true))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Childs));

            CreateMap<DicIPC, BaseTreeNodeDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => $"{src.Code} {src.NameRu ?? string.Empty}"))
                .ForMember(dest => dest.Selectable, opt => opt.UseValue(true))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Childs))
                .ForMember(dest => dest.Leaf, opt => opt.ResolveUsing<IpcTreeResolver>());

            CreateMap<DicICIS, BaseTreeNodeDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => $"{src.Description ?? string.Empty}"))
                .ForMember(dest => dest.Selectable, opt => opt.UseValue(true))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Childs));

        }

        internal class IpcTreeResolver : IValueResolver<DicIPC, BaseTreeNodeDto, bool?>
        {
            private readonly IExecutor _executor;
            public IpcTreeResolver(IExecutor executor)
            {
                _executor = executor;
            }

            public bool? Resolve(DicIPC source, BaseTreeNodeDto destination, bool? destMember, ResolutionContext context)
            {
                var children = _executor.GetQuery<GetDicIpcsByParentIdQuery>().Process(q => q.Execute(source.Id));

                return !children.Any();
            }
        }
    }
}