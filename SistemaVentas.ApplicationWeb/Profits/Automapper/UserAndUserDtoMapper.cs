using AutoMapper;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.User_Password_LoginDtos;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Profits.Automapper
{
    public class UserAndUserDtoMapper:Profile
    {
        public UserAndUserDtoMapper()
        {
            // MODEL USER TO VIEW MODEL DTO
            CreateMap<User, ViewModelUserDto>()
                 .ForMember(ModelDto =>
                  ModelDto.IdUser,
                  option => option.MapFrom(ModelEntity => ModelEntity.IdUser))
                  .ForMember(ModelDto =>
                  ModelDto.IdRol,
                  option => option.MapFrom(ModelEntity => ModelEntity.IdRol))
                   .ForMember(ModelDto =>
                  ModelDto.ItsActive,
                  option => option.MapFrom(ModelEntity => ModelEntity.ItsActive == true ? 1:0))
                  .ForMember(ModelDto =>
                  ModelDto.UrlPhoto,
                  option => option.MapFrom(ModelEntity => ModelEntity.UrlPhoto))
                  .ForMember(ModelDto =>
                  ModelDto.Email,
                  option => option.MapFrom(ModelEntity => ModelEntity.Email))
                   .ForMember(ModelDto =>
                  ModelDto.Phone,
                  option => option.MapFrom(ModelEntity => ModelEntity.Phone))
                   .ForMember(ModelDto =>
                  ModelDto.NameRol,
                  option => option.MapFrom(ModelEntity => ModelEntity.IdRolNavigation.Descriptions))
                  .ForMember(ModelDto =>
                  ModelDto.UserName,
                  option => option.MapFrom(ModelEntity => ModelEntity.UserName));

            // VIEW MODEL DTO TO MODEL USER
            CreateMap<ViewModelUserDto, User>()
                 .ForMember(ModelDto =>
                  ModelDto.IdRol,
                  option => option.MapFrom(ModelEntity => ModelEntity.IdRol))
                  .ForMember(ModelDto =>
                  ModelDto.IdUser,
                  option => option.MapFrom(ModelEntity => ModelEntity.IdUser))
                  .ForMember(ModelDto =>
                  ModelDto.ItsActive,
                  option => option.MapFrom(ModelEntity => ModelEntity.ItsActive == 1 ? true : false))
                  .ForMember(ModelDto =>
                  ModelDto.UrlPhoto,
                  option => option.MapFrom(ModelEntity => ModelEntity.UrlPhoto))
                  .ForMember(ModelDto =>
                  ModelDto.Email,
                  option => option.MapFrom(ModelEntity => ModelEntity.Email))
                   .ForMember(ModelDto =>
                  ModelDto.Phone,
                  option => option.MapFrom(ModelEntity => ModelEntity.Phone))
                   .ForMember(ModelDto =>
                  ModelDto.UserName,
                  option => option.MapFrom(ModelEntity => ModelEntity.UserName))
                    .ForMember(ModelDto =>
                  ModelDto.IdRolNavigation,
                  option => option.Ignore());
        }
    }
}
