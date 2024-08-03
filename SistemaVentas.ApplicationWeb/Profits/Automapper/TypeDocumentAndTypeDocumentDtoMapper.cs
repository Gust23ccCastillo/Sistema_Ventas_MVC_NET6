using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class TypeDocumentAndTypeDocumentDtoMapper:Profile
    {
        public TypeDocumentAndTypeDocumentDtoMapper()
        {
            CreateMap<TypeOfDocumentSale, ViewModelTypeOfDocumentSaleDto>()
             .ForMember(ModelDto =>
              ModelDto.IdTypeOfDocumentSale,
              option => option.MapFrom(ModelEntity => ModelEntity.IdTypeOfDocumentSale))
              .ForMember(ModelDto =>
              ModelDto.Descriptions,
              option => option.MapFrom(ModelEntity => ModelEntity.Descriptions)).ReverseMap();
        }
    }
}
