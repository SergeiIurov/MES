using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class ProductTypeMapperProfile : Profile
    {
        public ProductTypeMapperProfile()
        {
            CreateMap<ProductType, ProductTypeDto>().
                ForMember(dist => dist.Id, opt => opt.MapFrom(src => src.Id)).
                ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.Name));
        }

    }
}
