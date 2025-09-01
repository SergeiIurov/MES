using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class ProcessStateMapperProfile : Profile
    {
        public ProcessStateMapperProfile()
        {
            CreateMap<ProcessState, ProcessStateDto>()
                .ForMember(dist => dist.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dist => dist.ProductType, opt => opt.MapFrom(src => src.Station.ProductType));
        }
    }
}