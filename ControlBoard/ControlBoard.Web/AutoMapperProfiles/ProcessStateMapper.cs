using AutoMapper;
using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ControlBoard.Web.AutoMapperProfiles
{
    public class ProcessStateMapper : Profile
    {
        public ProcessStateMapper(MesDbContext context)
        {
        }
    }
}
