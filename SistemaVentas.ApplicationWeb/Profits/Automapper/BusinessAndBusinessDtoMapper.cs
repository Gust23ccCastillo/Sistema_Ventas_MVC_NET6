using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.BussineDtos;
using SistemaVentas.Entities;
using System.Globalization;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class BusinessAndBusinessDtoMapperL:Profile
    {
        public BusinessAndBusinessDtoMapperL()
        {
            CreateMap<Business, ViewModelBusinessDto>()
                  .ForMember(ModelDto =>
                  ModelDto.IdBusiness,
                  option => option.MapFrom(ModelEntity => ModelEntity.IdBusiness))
                  .ForMember(ModelDto =>
                  ModelDto.NameBusiness,
                  option => option.MapFrom(ModelEntity => ModelEntity.NameBusiness))
                  .ForMember(ModelDto =>
                  ModelDto.DocumentNumber,
                  option => option.MapFrom(ModelEntity => ModelEntity.DocumentNumber))
                  .ForMember(ModelDto =>
                  ModelDto.AddressBusiness,
                  option => option.MapFrom(ModelEntity => ModelEntity.AddressBusiness))
                  .ForMember(ModelDto =>
                  ModelDto.Email,
                  option => option.MapFrom(ModelEntity => ModelEntity.Email))
                  .ForMember(ModelDto =>
                  ModelDto.UrlLogo,
                  option => option.MapFrom(ModelEntity => ModelEntity.UrlLogo))
                  .ForMember(ModelDto =>
                  ModelDto.Coin,
                  option => option.MapFrom(ModelEntity => ModelEntity.Coin))
                  .ForMember(ModelDto =>
                  ModelDto.Phone,
                  option => option.MapFrom(ModelEntity => ModelEntity.Phone))
                  .ForMember(ModelDto =>
                  ModelDto.PercentageTax,
                  option => option.MapFrom(ModelEntity => Convert.ToString(ModelEntity.PercentageTax.Value, new CultureInfo("es-CR"))));

            CreateMap<ViewModelBusinessDto,Business>()
                .ForMember(ModelDto =>
                ModelDto.IdBusiness,
                option => option.MapFrom(ModelEntity => ModelEntity.IdBusiness))
                .ForMember(ModelDto =>
                ModelDto.NameBusiness,
                option => option.MapFrom(ModelEntity => ModelEntity.NameBusiness))
                .ForMember(ModelDto =>
                ModelDto.DocumentNumber,
                option => option.MapFrom(ModelEntity => ModelEntity.DocumentNumber))
                .ForMember(ModelDto =>
                ModelDto.AddressBusiness,
                option => option.MapFrom(ModelEntity => ModelEntity.AddressBusiness))
                .ForMember(ModelDto =>
                ModelDto.Email,
                option => option.MapFrom(ModelEntity => ModelEntity.Email))
                .ForMember(ModelDto =>
                ModelDto.UrlLogo,
                option => option.MapFrom(ModelEntity => ModelEntity.UrlLogo))
                .ForMember(ModelDto =>
                ModelDto.Coin,
                option => option.MapFrom(ModelEntity => ModelEntity.Coin))
                .ForMember(ModelDto =>
                ModelDto.Phone,
                option => option.MapFrom(ModelEntity => ModelEntity.Phone))
                .ForMember(ModelDto =>
                ModelDto.PercentageTax,
                option => option.MapFrom(ModelEntity => Convert.ToDecimal(ModelEntity.PercentageTax, new CultureInfo("es-CR"))));
        }
    }
}
