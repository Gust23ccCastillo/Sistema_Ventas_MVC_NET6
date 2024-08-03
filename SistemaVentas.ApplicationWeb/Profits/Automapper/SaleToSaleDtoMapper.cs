using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos;
using SistemaVentas.Entities;
using System.Globalization;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class SaleToSaleDtoMapper:Profile
    {
        public SaleToSaleDtoMapper()
        {
            CreateMap<Sale, ViewModelSaleDto>()
             .ForMember(ModelDto =>
              ModelDto.IdSale,
              option => option.MapFrom(ModelEntity => ModelEntity.IdSale))
              .ForMember(ModelDto =>
              ModelDto.SalesNumber,
              option => option.MapFrom(ModelEntity => ModelEntity.SalesNumber))
               .ForMember(ModelDto =>
              ModelDto.IdTypeOfDocumentSale,
              option => option.MapFrom(ModelEntity => ModelEntity.IdTypeOfDocumentSale))
                .ForMember(ModelDto =>
              ModelDto.TypeOfoDocumentSale,
              option => option.MapFrom(ModelEntity => ModelEntity.IdTypeOfDocumentSaleNavigation.Descriptions))
                 .ForMember(ModelDto =>
              ModelDto.IdUser,
              option => option.MapFrom(ModelEntity => ModelEntity.IdUser))
                  .ForMember(ModelDto =>
              ModelDto.UserProfile,
              option => option.MapFrom(ModelEntity => ModelEntity.IdUserNavigation.UserName))
                   .ForMember(ModelDto =>
              ModelDto.ClientDocument,
              option => option.MapFrom(ModelEntity => ModelEntity.ClientDocument))
                    .ForMember(ModelDto =>
              ModelDto.ClientName,
              option => option.MapFrom(ModelEntity => ModelEntity.ClientName))
                     .ForMember(ModelDto =>
              ModelDto.SubTotal,
              option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.SubTotal.Value, new CultureInfo("es-CR"))))
                      .ForMember(ModelDto =>
              ModelDto.TotalTax,
              option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.TotalTax.Value, new CultureInfo("es-CR"))))
                 .ForMember(ModelDto =>
              ModelDto.Total,
              option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.Total.Value, new CultureInfo("es-CR"))))
                    .ForMember(ModelDto =>
              ModelDto.RegistrationDate,
              option => option.MapFrom(ModelEntity => ModelEntity.RegistrationDate.Value.ToString("dd/MM/yyyy")));

            //CONVERSION DE VENTA DETALLE DTO A VENTA DETALLE MODELO
            CreateMap<ViewModelSaleDetailDto, SaleDetail>()
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
               option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.Price)))
             .ForMember(ModelDto =>
               ModelDto.Total,
               option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.Total)));

            CreateMap<ViewModelSaleDto, Sale>()
                 .ForMember(ModelDto =>
              ModelDto.SaleDetails,
              option => option.MapFrom(ModelEntity => ModelEntity.SaleDetails))
                  .ForMember(ModelDto =>
              ModelDto.RegistrationDate,
              option => option.MapFrom(ModelEntity => Convert.ToDateTime(ModelEntity.RegistrationDate).ToString("dd/MM/yyyy")))
                  .ForMember(ModelDto =>
             ModelDto.IdUserNavigation,
             option => option.Ignore())
                  .ForMember(ModelDto =>
             ModelDto.IdTypeOfDocumentSaleNavigation,
             option => option.Ignore())
                .ForMember(ModelDto =>
              ModelDto.IdSale,
              option => option.MapFrom(ModelEntity => ModelEntity.IdSale))
              .ForMember(ModelDto =>
              ModelDto.SalesNumber,
              option => option.MapFrom(ModelEntity => ModelEntity.SalesNumber))
               .ForMember(ModelDto =>
              ModelDto.IdTypeOfDocumentSale,
              option => option.MapFrom(ModelEntity => ModelEntity.IdTypeOfDocumentSale))
                 .ForMember(ModelDto =>
              ModelDto.IdUser,
              option => option.MapFrom(ModelEntity => ModelEntity.IdUser))
                   .ForMember(ModelDto =>
              ModelDto.ClientDocument,
              option => option.MapFrom(ModelEntity => ModelEntity.ClientDocument))
                    .ForMember(ModelDto =>
              ModelDto.ClientName,
              option => option.MapFrom(ModelEntity => ModelEntity.ClientName))
                     .ForMember(ModelDto =>
              ModelDto.SubTotal,
              option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.SubTotal).ToString()))
                      .ForMember(ModelDto =>
              ModelDto.TotalTax,
              option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.TotalTax)))
                 .ForMember(ModelDto =>
              ModelDto.Total,
              option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.Total)));
                    

        }
    }
}
