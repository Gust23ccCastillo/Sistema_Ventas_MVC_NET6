using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.BussineDtos;
using SistemaVentas.ApplicationWeb.Profits.Response;
using SistemaVentas.BusinessLogic.Interface;
using Newtonsoft.Json;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Controllers
{
    public class BusinessApplicationController : Controller
    {
        private readonly IMapper _Automapper;
        private readonly IServicesBusiness _ServicesBusiness;

        public BusinessApplicationController(IMapper automapper, 
            IServicesBusiness servicesBusiness)
        {
            _Automapper = automapper;
            _ServicesBusiness = servicesBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBusinessInformation()
        {
            GenericResponse<ViewModelBusinessDto> _ResponseForUser = new ();
            try
            {
                var GetInformationForBusiness = this._Automapper.Map<ViewModelBusinessDto>(await this._ServicesBusiness.GetApplicationBusiness());
                _ResponseForUser.State = true;
                _ResponseForUser.Object = GetInformationForBusiness;

            }
            catch (Exception ex)
            {
               _ResponseForUser.State = false;
                _ResponseForUser.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, _ResponseForUser);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChangesBusinessInformation([FromForm] IFormFile logo, [FromForm] string model)
        {
            GenericResponse<ViewModelBusinessDto> _responseForUser = new();
            try
            {
               
                //REVISION DE CREACION DE IMAGEN
                string nameLogo = string.Empty;
                Stream logoStream = Stream.Null;
               if(logo != null)
                {
                    string name_And_Code = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(logo.FileName);
                    nameLogo = string.Concat(name_And_Code, extension);
                    logoStream = logo.OpenReadStream();
                    
                }

                var ResultForEditBusiness = await this._ServicesBusiness.SaveChangesBusiness(
                    this._Automapper.Map<Business>(JsonConvert.DeserializeObject<ViewModelBusinessDto>(model)), logoStream, nameLogo);
               
                _responseForUser.State = true;
                _responseForUser.Object = this._Automapper.Map<ViewModelBusinessDto>(ResultForEditBusiness);
            }
            catch (Exception ex)
            {
                _responseForUser.State = false;
                _responseForUser.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, _responseForUser);
        }
    }
}
