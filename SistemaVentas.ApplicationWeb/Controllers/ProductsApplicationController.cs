using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.ProductDtos;
using SistemaVentas.ApplicationWeb.Profits.Response;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.ApplicationWeb.Controllers
{
    public class ProductsApplicationController : Controller
    {
        private readonly IMapper _automapperInject;
        private readonly IServicesProductApplication _servicesProductInject;

        public ProductsApplicationController(IMapper automapperInject, 
            IServicesProductApplication servicesProductInject)
        {
            _automapperInject = automapperInject;
            _servicesProductInject = servicesProductInject;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListProducts()
        {
            var GetProducts = await this._servicesProductInject.GetAllProducts();
            var ReturnModel = this._automapperInject.Map<List<ViewModelProductDto>>(GetProducts);

            return StatusCode(StatusCodes.Status200OK,
                new { data = ReturnModel });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] IFormFile imageProduct, [FromForm] string model)
        {
            GenericResponse<ViewModelProductDto> ResponseModel = new();
            try
            {
                string imageName = string.Empty;
                Stream imageStream = Stream.Null;
                if(imageProduct != null)
                {
                    imageName = string.Concat(Guid.NewGuid().ToString("N"),
                        Path.GetExtension(imageProduct.FileName));
                    imageStream = imageProduct.OpenReadStream();

                }
                var modelJson = JsonConvert.DeserializeObject<ViewModelProductDto>(model);

                var ResultToCreate = await this._servicesProductInject.CreateProductInApplication(
                    this._automapperInject.Map<Product>(modelJson),
                    imageStream,imageName);

                ResponseModel.State = true;
                ResponseModel.Object = this._automapperInject.Map<ViewModelProductDto>(ResultToCreate);
            }
            catch(Exception ex)
            {
                ResponseModel.State = false;
                ResponseModel.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, ResponseModel);
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct([FromForm] IFormFile imageProduct, [FromForm] string model)
        {
            GenericResponse<ViewModelProductDto> ResponseModel = new();
            try
            {
                Stream imageStream = Stream.Null;
                string imageName = string.Empty;
                if (imageProduct != null)
                {

                    imageName = string.Concat(Guid.NewGuid().ToString("N"),
                       Path.GetExtension(imageProduct.FileName));
                    imageStream = imageProduct.OpenReadStream();
                }

                var ResultToEdit = await this._servicesProductInject.EditProductInApplication(
                       this._automapperInject.Map<Product>(JsonConvert.DeserializeObject<ViewModelProductDto>(model)),
                       imageStream, imageName);

                ResponseModel.State = true;
                ResponseModel.Object = this._automapperInject.Map<ViewModelProductDto>(ResultToEdit);
            }
            catch (Exception ex)
            {

                ResponseModel.State = false;
                ResponseModel.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, ResponseModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int idProduct)
        {
            GenericResponse<ViewModelProductDto> ResponseModel = new();
            try
            {
                ResponseModel.State = await this._servicesProductInject.RemoveProductInApplication(idProduct);

            }
            catch (Exception ex)
            {

                ResponseModel.State = false;
                ResponseModel.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, ResponseModel);

        }

    }

    

    
}
