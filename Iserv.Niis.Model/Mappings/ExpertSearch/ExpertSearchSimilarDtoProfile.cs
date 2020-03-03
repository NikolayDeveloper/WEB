using System;
using AutoMapper; 
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.ExpertSearch;

namespace Iserv.Niis.Model.Mappings.ExpertSearch
{
	public class ExpertSearchSimilarDtoProfile : Profile
	{
		public ExpertSearchSimilarDtoProfile()
		{
		    CreateMap<ExpertSearchSimilar, ExpertSearchSimilarDto>();

		    CreateMap<ExpertSearchSimilarDto, ExpertSearchSimilar>()
		        .ForMember(s => s.Id, opt => opt.UseValue(0))
                .ForMember(s => s.DateCreate, opt => opt.UseValue(DateTimeOffset.Now));
        }
	}
}