using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos;
using SistemaVentas.Entities;
using System.Globalization;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class SaleDetailAndReportSaleDtoMapper:Profile
    {
        public SaleDetailAndReportSaleDtoMapper()
        {
            CreateMap<SaleDetail, ViewModelReportSaleDto>()
            .ForMember(ModelDto =>
            ModelDto.RegistrationDate,
            option => option.MapFrom(ModelEntity => ModelEntity.IdSaleNavigation.RegistrationDate.Value.ToString("dd/MM/yyyy")))
            .ForMember(ModelDto =>
            ModelDto.NumberSale,
            option => option.MapFrom(ModelEntity => ModelEntity.IdSaleNavigation.SalesNumber))
            .ForMember(ModelDto =>
            ModelDto.TypeOfDocument,
            option => option.MapFrom(ModelEntity => ModelEntity.IdSaleNavigation.IdTypeOfDocumentSaleNavigation.Descriptions))
            .ForMember(ModelDto =>
            ModelDto.ClientDocument,
            option => option.MapFrom(ModelEntity => ModelEntity.IdSaleNavigation.ClientDocument))
            .ForMember(ModelDto =>
            ModelDto.ClientName,
            option => option.MapFrom(ModelEntity => ModelEntity.IdSaleNavigation.ClientName))
            .ForMember(ModelDto =>
            ModelDto.SubTotal,
            option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.IdSaleNavigation.SubTotal.Value, new CultureInfo("es-CR"))))
            .ForMember(ModelDto =>
            ModelDto.TotalTax,
            option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.IdSaleNavigation.TotalTax.Value, new CultureInfo("es-CR"))))
            .ForMember(ModelDto =>
            ModelDto.TotalSale,
            option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.IdSaleNavigation.Total.Value, new CultureInfo("es-CR"))))
            .ForMember(ModelDto =>
            ModelDto.Product,
            option => option.MapFrom(ModelEntity => ModelEntity.ProductMark))
            .ForMember(ModelDto =>
            ModelDto.Description,
            option => option.MapFrom(ModelEntity => ModelEntity.ProductDescription))
            .ForMember(ModelDto =>
            ModelDto.Price,
            option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.Price.Value, new CultureInfo("es-CR"))))
            .ForMember(ModelDto =>
            ModelDto.Total,
            option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.Total.Value, new CultureInfo("es-CR"))))
            .ForMember(ModelDto =>
            ModelDto.Quantity,
            option => option.MapFrom(ModelEntity => ModelEntity.Quantity));


        }
    }
}
