using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class CarExecutionProfile : Profile
    {
        public CarExecutionProfile()
        {
            CreateMap<CarExecution, CarExecutionDto>().
                ForMember(dist => dist.Id, opt => opt.MapFrom(src => src.Id)).
                ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.Name)).
                ForMember(dist => dist.Code, opt => opt.MapFrom(src => src.Code));
        }
    }
}
