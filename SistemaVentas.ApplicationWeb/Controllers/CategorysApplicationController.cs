using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.CategoryDtos;
using SistemaVentas.ApplicationWeb.Profits.Response;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Controllers
{
    public class CategorysApplicationController : Controller
    {
        private readonly IMapper _automapperInject;
        private readonly IServicesCategoryApplication _servicesCategoryInject;

        public CategorysApplicationController(IMapper automapperInject, 
            IServicesCategoryApplication servicesCategoryInject)
        {
            _automapperInject = automapperInject;
            _servicesCategoryInject = servicesCategoryInject;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategorys()
        {
            List<ViewModelCategoryDto> modelCategoryDtos = this._automapperInject.Map<List<ViewModelCategoryDto>>(
                await this._servicesCategoryInject.GetListApplicationCategories());
            return StatusCode(StatusCodes.Status200OK, new {data =  modelCategoryDtos});
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] ViewModelCategoryDto modelCategoryDto)
        {
            GenericResponse<ViewModelCategoryDto> genericResponse = new();
            try
            {
                var ResultToOperation = await this._servicesCategoryInject.CreateCategoryApplication(
                    this._automapperInject.Map<Category>(modelCategoryDto));

                genericResponse.State = true;
                genericResponse.Object = this._automapperInject.Map<ViewModelCategoryDto>(ResultToOperation);
            }
            catch
            {
                genericResponse.State = false;
                genericResponse.Message = "Problemas en el Servidor, Porfavor Intentalo mas tarde!!";

            }
            return StatusCode(StatusCodes.Status200OK, genericResponse);

        }


        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] ViewModelCategoryDto modelCategoryDto)
        {
            GenericResponse<ViewModelCategoryDto> genericResponse = new();
            try
            {
                var ResultToOperation = await this._servicesCategoryInject.UpdateCategoryApplication(
                    this._automapperInject.Map<Category>(modelCategoryDto));

                genericResponse.State = true;
                genericResponse.Object = this._automapperInject.Map<ViewModelCategoryDto>(ResultToOperation);
            }
            catch
            {
                genericResponse.State = false;
                genericResponse.Message = "Problemas en el Servidor, Porfavor Intentalo mas tarde!!";

            }
            return StatusCode(StatusCodes.Status200OK, genericResponse);

        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCategory(int idCategory)
        {
            GenericResponse<string> genericResponse = new();
            try
            {
                genericResponse.State = await this._servicesCategoryInject.RemoveCategoryApplication(idCategory);          
            }
            catch
            {
                genericResponse.State = false;
                genericResponse.Message = "Problemas en el Servidor, Porfavor Intentalo mas tarde!!";

            }
            return StatusCode(StatusCodes.Status200OK, genericResponse);

        }
    }
}
