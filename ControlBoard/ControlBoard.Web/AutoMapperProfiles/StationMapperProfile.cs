using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class StationMapperProfile : Profile
    {
        public StationMapperProfile()
        {
            CreateMap<Station, StationDto>().
                ForMember(dist => dist.Id, opt => opt.MapFrom(src => src.Id)).
                ForMember(dist => dist.ChartElementId, opt => opt.MapFrom(src => src.ChartElementId)).
                ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.Name)).
                ForMember(dist => dist.AreaId, opt => opt.MapFrom(src => src.Area.Id)).
                ForMember(dist => dist.AreaName, opt => opt.MapFrom(src => src.Area.Name)).
                ForMember(dist => dist.ProductType, opt => opt.MapFrom(src => src.ProductType)).
                ForMember(dist => dist.ProcessStates, opt => opt.MapFrom(src => src.ProcessStates));

        }
    }
}
