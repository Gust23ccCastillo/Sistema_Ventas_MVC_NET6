using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.GeneralDtos;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class MenuAndMenuDtoMapper:Profile
    {
        public MenuAndMenuDtoMapper()
        {
            CreateMap<Menu, ViewModelMenuDto>()
              .ForMember(ModelDto =>
              ModelDto.SubMenus,
              option => option.MapFrom(ModelEntity => ModelEntity.InverseIdMenuFatherNavigation)); 
        }
    }
}
