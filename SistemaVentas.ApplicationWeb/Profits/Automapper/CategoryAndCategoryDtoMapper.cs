using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.CategoryDtos;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class CategoryAndCategoryDtoMapper:Profile
    {
        public CategoryAndCategoryDtoMapper()
        {
            CreateMap<Category, ViewModelCategoryDto>()
            .ForMember(ModelDto =>
              ModelDto.IdCategoria,
              option => option.MapFrom(ModelEntity => ModelEntity.IdCategoria))
            .ForMember(ModelDto =>
              ModelDto.Descriptions,
              option => option.MapFrom(ModelEntity => ModelEntity.Descriptions))
            .ForMember(ModelDto =>
              ModelDto.ItsActive,
              option => option.MapFrom(ModelEntity => ModelEntity.ItsActive == true ? 1 : 0));

            CreateMap<ViewModelCategoryDto, Category>()
           .ForMember(ModelDto =>
             ModelDto.IdCategoria,
             option => option.MapFrom(ModelEntity => ModelEntity.IdCategoria))
           .ForMember(ModelDto =>
             ModelDto.Descriptions,
             option => option.MapFrom(ModelEntity => ModelEntity.Descriptions))
           .ForMember(ModelDto =>
             ModelDto.ItsActive,
             option => option.MapFrom(ModelEntity => ModelEntity.ItsActive == 1 ? true : false));
        }
    }
}
