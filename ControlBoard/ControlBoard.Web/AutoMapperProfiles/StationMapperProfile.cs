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
                ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
