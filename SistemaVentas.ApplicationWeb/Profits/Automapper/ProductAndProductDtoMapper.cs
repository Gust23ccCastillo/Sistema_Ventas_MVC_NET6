using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.ProductDtos;
using SistemaVentas.Entities;
using System.Globalization;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class ProductAndProductDtoMapper:Profile
    {
        public ProductAndProductDtoMapper()
        {
            CreateMap<Product, ViewModelProductDto>()
                .ForMember(ModelDto =>
              ModelDto.IdProduct,
              option => option.MapFrom(ModelEntity => ModelEntity.IdProduct))
                 .ForMember(ModelDto =>
              ModelDto.IdCategoria,
              option => option.MapFrom(ModelEntity => ModelEntity.IdCategoria))
                 .ForMember(ModelDto =>
              ModelDto.NameCategory,
              option => option.MapFrom(ModelEntity => ModelEntity.IdCategoriaNavigation.Descriptions))
                 .ForMember(ModelDto =>
              ModelDto.Mark,
              option => option.MapFrom(ModelEntity => ModelEntity.Mark))
                  .ForMember(ModelDto =>
              ModelDto.Barcode,
              option => option.MapFrom(ModelEntity => ModelEntity.Barcode))
                   .ForMember(ModelDto =>
              ModelDto.Descriptions,
              option => option.MapFrom(ModelEntity => ModelEntity.Descriptions))
                   .ForMember(ModelDto =>
              ModelDto.Stock,
              option => option.MapFrom(ModelEntity => ModelEntity.Stock))
                    .ForMember(ModelDto =>
              ModelDto.UrlImage,
              option => option.MapFrom(ModelEntity => ModelEntity.UrlImage))
                    .ForMember(ModelDto =>
              ModelDto.Price,
              option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.Price.Value,new CultureInfo("es-CR"))))
                    .ForMember(ModelDto =>
              ModelDto.ItsActive,
              option => option.MapFrom(ModelEntity => ModelEntity.ItsActive == true ? 1 : 0));

            CreateMap<ViewModelProductDto,Product>()
               .ForMember(ModelDto =>
             ModelDto.IdProduct,
             option => option.MapFrom(ModelEntity => ModelEntity.IdProduct))
                .ForMember(ModelDto =>
             ModelDto.IdCategoria,
             option => option.MapFrom(ModelEntity => ModelEntity.IdCategoria))
                .ForMember(ModelDto =>
             ModelDto.IdCategoriaNavigation,
             option => option.Ignore())
                .ForMember(ModelDto =>
             ModelDto.Mark,
             option => option.MapFrom(ModelEntity => ModelEntity.Mark))
                 .ForMember(ModelDto =>
             ModelDto.Barcode,
             option => option.MapFrom(ModelEntity => ModelEntity.Barcode))
                  .ForMember(ModelDto =>
             ModelDto.Descriptions,
             option => option.MapFrom(ModelEntity => ModelEntity.Descriptions))
                  .ForMember(ModelDto =>
             ModelDto.Stock,
             option => option.MapFrom(ModelEntity => ModelEntity.Stock))
                   .ForMember(ModelDto =>
             ModelDto.UrlImage,
             option => option.MapFrom(ModelEntity => ModelEntity.UrlImage))
                   .ForMember(ModelDto =>
             ModelDto.Price,
             option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.Price, new CultureInfo("es-CR"))))
                   .ForMember(ModelDto =>
             ModelDto.ItsActive,
             option => option.MapFrom(ModelEntity => ModelEntity.ItsActive == 1 ? true : false));

        }
    }
}
