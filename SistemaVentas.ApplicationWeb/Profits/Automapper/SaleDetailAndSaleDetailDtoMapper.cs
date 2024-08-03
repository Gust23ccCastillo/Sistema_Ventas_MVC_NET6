using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos;
using SistemaVentas.Entities;
using System.Globalization;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class SaleDetailAndSaleDetailDtoMapper:Profile
    {
        public SaleDetailAndSaleDetailDtoMapper()
        {
            CreateMap<SaleDetail, ViewModelSaleDetailDto>()
               .ForMember(ModelDto =>
                 ModelDto.IdProduct,
                 option => option.MapFrom(ModelEntity => ModelEntity.IdProduct))
               .ForMember(ModelDto =>
                 ModelDto.ProductMark,
                 option => option.MapFrom(ModelEntity => ModelEntity.ProductMark))
               .ForMember(ModelDto =>
                 ModelDto.ProductDescription,
                 option => option.MapFrom(ModelEntity => ModelEntity.ProductDescription))
               .ForMember(ModelDto =>
                 ModelDto.ProductCategory,
                 option => option.MapFrom(ModelEntity => ModelEntity.ProductCategory))
               .ForMember(ModelDto =>
                 ModelDto.Quantity,
                 option => option.MapFrom(ModelEntity => ModelEntity.Quantity))
               .ForMember(ModelDto =>
                 ModelDto.Price,
                 option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.Price.Value,new CultureInfo("es-CR"))))
               .ForMember(ModelDto =>
                 ModelDto.Total,
                 option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.Total.Value, new CultureInfo("es-CR"))));

            CreateMap<ViewModelSaleDetailDto,SaleDetail>()
              .ForMember(ModelDto =>
                ModelDto.IdProduct,
                option => option.MapFrom(ModelEntity => ModelEntity.IdProduct))
              .ForMember(ModelDto =>
                ModelDto.ProductMark,
                option => option.MapFrom(ModelEntity => ModelEntity.ProductMark))
              .ForMember(ModelDto =>
                ModelDto.ProductDescription,
                option => option.MapFrom(ModelEntity => ModelEntity.ProductDescription))
              .ForMember(ModelDto =>
                ModelDto.ProductCategory,
                option => option.MapFrom(ModelEntity => ModelEntity.ProductCategory))
              .ForMember(ModelDto =>
                ModelDto.Quantity,
                option => option.MapFrom(ModelEntity => ModelEntity.Quantity))
              .ForMember(ModelDto =>
                ModelDto.Price,
                option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.Price, new CultureInfo("es-CR"))))
              .ForMember(ModelDto =>
                ModelDto.Total,
                option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.Total, new CultureInfo("es-CR"))));

            
        }
    }

}
