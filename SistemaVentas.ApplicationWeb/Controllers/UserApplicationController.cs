using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.GeneralDtos;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.User_Password_LoginDtos;
using SistemaVentas.ApplicationWeb.Profits.Response;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Controllers
{
    public class UserApplicationController : Controller
    {
        private readonly IMapper _Automapper;
        private readonly IServicesUsers _ServicesUsers;
        private readonly IServicesRolesApplication _ServicesRoles;

        public UserApplicationController(IMapper automapper, 
            IServicesUsers servicesUsers, IServicesRolesApplication servicesRoles)
        {
            _Automapper = automapper;
            _ServicesUsers = servicesUsers;
            _ServicesRoles = servicesRoles;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListAllRol()
        {
            List<ViewModelRolDto> modelRolDtos = this._Automapper.Map<List<ViewModelRolDto>>(await this._ServicesRoles.ListRolsInApplication());
            return StatusCode(StatusCodes.Status200OK, modelRolDtos);
        }

        [HttpGet]
        public async Task<IActionResult> ListAllUsers()
        {
            List<ViewModelUserDto> modelUserslDtos = this._Automapper.Map<List<ViewModelUserDto>>(await this._ServicesUsers.GetListUsersInApplication());
            return StatusCode(StatusCodes.Status200OK, new {data = modelUserslDtos});
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserProfile([FromForm] IFormFile PhotoUser, [FromForm] string ModelEntity)
        {
            GenericResponse<ViewModelUserDto> ModelResponse = new GenericResponse<ViewModelUserDto>();
            try
            {
                if(PhotoUser == null)
                {
                    ModelResponse.Message = "Debe Agregar una Foto de Perfil de Usuario!!";
                    return StatusCode(StatusCodes.Status200OK,ModelResponse);
                }
                string NamePhoto = string.Empty;
                Stream PhotoUserArchive = Stream.Null;
                string UrlTemplateMail = $"{this.Request.Scheme}://{this.Request.Host}/TemplatesForServices/SendUserPassword?EmailParameter=[email]&PasswordParameter=[password]";
                User NewUserEntity = new User();

                if (PhotoUser != null)
                {

                    NewUserEntity = await CreateUserProfileWithPhoto(PhotoUser, JsonConvert.DeserializeObject<ViewModelUserDto>(ModelEntity), PhotoUserArchive, UrlTemplateMail);
                }
                else
                {
                    NewUserEntity = await this._ServicesUsers.CreateUserInApplication(this._Automapper.Map<User>(JsonConvert.DeserializeObject<ViewModelUserDto>(ModelEntity)), PhotoUserArchive, NamePhoto, UrlTemplateMail);
                }
                
                ModelResponse.State = true;
                ModelResponse.Message = string.Empty;
                ModelResponse.Object = this._Automapper.Map<ViewModelUserDto>(NewUserEntity);
            }
            catch
            {

                ModelResponse.State = false;
                ModelResponse.Message = "Ocurrio un Problema en el Servidor!!, Porfavor Intentarlo mas tarde.";
                ModelResponse.Object = null;
            }

            return StatusCode(StatusCodes.Status200OK,ModelResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromForm] IFormFile PhotoUser, [FromForm] string ModelEntity)
        {
            GenericResponse<ViewModelUserDto> ModelResponse = new GenericResponse<ViewModelUserDto>();
            try
            {
                string NamePhoto = string.Empty;
                Stream PhotoUserArchive = Stream.Null;
                User EditUserEntity = new User();

                if (PhotoUser != null)
                {

                    EditUserEntity = await UpdateUserProfileWithPhoto(PhotoUser, JsonConvert.DeserializeObject<ViewModelUserDto>(ModelEntity), PhotoUserArchive);
                }
                else
                {
                    EditUserEntity = await this._ServicesUsers.UpdateUserInApplication(this._Automapper.Map<User>(JsonConvert.DeserializeObject<ViewModelUserDto>(ModelEntity)), PhotoUserArchive, NamePhoto);
                }

                ModelResponse.State = true;
                ModelResponse.Message = string.Empty;
                ModelResponse.Object = this._Automapper.Map<ViewModelUserDto>(EditUserEntity);
            }
            catch
            {

                ModelResponse.State = false;
                ModelResponse.Message = "Ocurrio un Problema en el Servidor!!, Porfavor Intentarlo mas tarde.";
                ModelResponse.Object = null;
            }

            return StatusCode(StatusCodes.Status200OK, ModelResponse);
        }

        private async Task<User> UpdateUserProfileWithPhoto(IFormFile PhotoUser,ViewModelUserDto viewModelUserDto, Stream PhotoUserArchive)
        {
           
            string NamePhoto = string.Concat(Guid.NewGuid().ToString("N"), Path.GetExtension(PhotoUser.FileName));
            PhotoUserArchive = PhotoUser.OpenReadStream();
            return await this._ServicesUsers.UpdateUserInApplication(this._Automapper.Map<User>(viewModelUserDto), PhotoUserArchive, NamePhoto);
        }

        private async Task<User> CreateUserProfileWithPhoto(IFormFile PhotoUser, ViewModelUserDto viewModelUserDto, Stream PhotoUserArchive,string UrlTemplateMail)
        {

            string NamePhoto = string.Concat(Guid.NewGuid().ToString("N"), Path.GetExtension(PhotoUser.FileName));
            PhotoUserArchive = PhotoUser.OpenReadStream();
            return await this._ServicesUsers.CreateUserInApplication(this._Automapper.Map<User>(viewModelUserDto), PhotoUserArchive, NamePhoto,UrlTemplateMail);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserProfile(int IdUserProfileParameter)
        {
            GenericResponse<string> ModelResponse = new GenericResponse<string>();
            try
            {
                ModelResponse.State = await this._ServicesUsers.RemoveUserInApplication(IdUserProfileParameter);
            }
            catch (Exception)
            {

                ModelResponse.State = false;
                ModelResponse.Message = "Problemas en el Servidor para Eliminar el Usuario";
            }
            return StatusCode(StatusCodes.Status200OK, ModelResponse);

        }





    }

    

}
