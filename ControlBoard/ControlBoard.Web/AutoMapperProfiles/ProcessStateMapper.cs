using AutoMapper;
using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class ProcessStateAdvMapper : Profile
    {
        public ProcessStateAdvMapper()
        {
            CreateMap<ProcessState, ProcessStateAdvDto>()
                .ForMember(dist => dist.StationId, opt => opt.MapFrom(src => src.StationId))
                .ForMember(dist => dist.Value, opt => opt.MapFrom(src => src.Value));
        }
    }
}
