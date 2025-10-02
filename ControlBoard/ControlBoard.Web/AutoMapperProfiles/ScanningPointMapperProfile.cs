using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class ScanningPointMapperProfile:Profile
    {
        public ScanningPointMapperProfile()
        {
            CreateMap<ScanningPoint, ScanningPointDto>().ForMember(dist => dist.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dist => dist.OrderNum, opt => opt.MapFrom(src => src.OrderNum))
                .ForMember(dist => dist.LineNumber, opt => opt.MapFrom(src => src.LineNumber))
                .ForMember(dist => dist.CodeTs, opt => opt.MapFrom(src => src.CodeTs));
        }
    }
}
