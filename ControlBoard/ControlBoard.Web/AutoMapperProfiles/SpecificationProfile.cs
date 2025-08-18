using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class SpecificationProfile : Profile
    {
        public SpecificationProfile()
        {
            CreateMap<Specification, SpecificationDto>()
                .ForMember(dist => dist.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dist => dist.SequenceNumber, opt => opt.MapFrom(src => src.SequenceNumber))
                .ForMember(dist => dist.SpecificationStr, opt => opt.MapFrom(src => src.SpecificationStr));
        }
    }
}
