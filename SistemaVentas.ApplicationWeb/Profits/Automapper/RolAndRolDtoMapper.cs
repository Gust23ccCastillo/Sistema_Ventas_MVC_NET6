using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.GeneralDtos;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class RolAndRolDtoMapper:Profile
    {
        public RolAndRolDtoMapper()
        {
            CreateMap<Rol, ViewModelRolDto>()
                .ForMember(ModelDto =>
                  ModelDto.IdRol,
                  option => option.MapFrom(ModelEntity => ModelEntity.IdRol))
                .ForMember(ModelDto =>
                  ModelDto.Descriptions,
                  option => option.MapFrom(ModelEntity => ModelEntity.Descriptions)).ReverseMap();
        }
    }
}
