using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class AreaMapperProfile : Profile
    {
        public AreaMapperProfile()
        {
            CreateMap<Area, AreaDto>().ForMember(dist => dist.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dist => dist.Range, opt => opt.MapFrom(src => src.Range))
                .ForMember(dist => dist.Stations, opt => opt.MapFrom(src => src.Stations))
                .ForMember(dist => dist.IsDisabled, opt => opt.MapFrom(src => src.IsDisabled));
        }
    }
}