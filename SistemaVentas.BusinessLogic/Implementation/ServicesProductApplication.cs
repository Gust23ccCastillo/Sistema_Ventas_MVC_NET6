using Microsoft.EntityFrameworkCore;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesProductApplication : IServicesProductApplication
    {
        private readonly IGenericPrincipalRepository<Product> _genericPrincipalRepository;
        private readonly IServicesFirebaseStorage _servicesFirebaseStorage;
       

        public ServicesProductApplication(IGenericPrincipalRepository<Product> genericPrincipalRepository,
            IServicesFirebaseStorage servicesFirebaseStorage
           )
        {
            _genericPrincipalRepository = genericPrincipalRepository;
            _servicesFirebaseStorage = servicesFirebaseStorage;
           
        }

        public async Task<List<Product>> GetAllProducts()
        {
            IQueryable<Product> GetAllProducts = await this._genericPrincipalRepository.ConsultSpecificInformation();
            return GetAllProducts.Include(IncludeInfo => IncludeInfo.IdCategoriaNavigation).ToList();
           // return GetAllProducts.ToList();
        }
        public async Task<Product> CreateProductInApplication(Product product, Stream image = null, string imageName = "")
        {
            try
            {
                var existingProduct = await this._genericPrincipalRepository.GetSpecificInformation(SearchProduct =>
                SearchProduct.Barcode == product.Barcode);

                if (existingProduct != null)
                    throw new TaskCanceledException("El codigo de barra ya existe");

                product.NameImage = imageName;
                if(image != null)
                {
                    string GetUrlImage = await this._servicesFirebaseStorage.UploadStorage(image, "carpeta_producto",imageName);
                    product.UrlImage = GetUrlImage;
                }

                var ResultToCreateProduct = await this._genericPrincipalRepository.CreateSpecificInformation(product);
                if(ResultToCreateProduct.IdProduct == 0)
                    throw new TaskCanceledException("Problemas para crear el producto, Porfavor Intentar de nuevo");

                IQueryable<Product> VerifyProductCreate = await this._genericPrincipalRepository.ConsultSpecificInformation(SearchProduct =>
                SearchProduct.IdProduct == ResultToCreateProduct.IdProduct);

                ResultToCreateProduct = VerifyProductCreate.Include(IncludeInfo => IncludeInfo.IdCategoriaNavigation).First();

                return ResultToCreateProduct;
            }
            catch
            {

                throw;
            }
        }

        public async Task<Product> EditProductInApplication(Product product, Stream image = null, string imageName = "")
        {
            try
            {
                var existingProduct = await this._genericPrincipalRepository.GetSpecificInformation(SearchProduct =>
                SearchProduct.Barcode == product.Barcode && SearchProduct.IdProduct != product.IdProduct);

                if (existingProduct != null)
                    throw new TaskCanceledException("El codigo de barra ya existe");
                IQueryable<Product> GetProduct = await this._genericPrincipalRepository.ConsultSpecificInformation(Search => 
                Search.IdProduct == product.IdProduct);

                Product updatedProduct = GetProduct.First();

                updatedProduct.Barcode = product.Barcode;
                updatedProduct.Mark = product.Mark;
                updatedProduct.Descriptions = product.Descriptions;
                updatedProduct.IdCategoria = product.IdCategoria;
                updatedProduct.Stock = product.Stock;
                updatedProduct.Price = product.Price;
                updatedProduct.ItsActive = product.ItsActive;
                

                if(image != null)
                {
                    await this._servicesFirebaseStorage.RemoveStorage("carpeta_producto", updatedProduct.NameImage);
                    updatedProduct.NameImage = imageName;
                    updatedProduct.UrlImage = await this._servicesFirebaseStorage.UploadStorage(image, "carpeta_producto", updatedProduct.NameImage);
                   
                }

                var ResultToOperationUpdate = await this._genericPrincipalRepository.UpdateSpecificInformation(updatedProduct);
                if(!ResultToOperationUpdate)
                    throw new TaskCanceledException("Problemas para editar el producto");

                return GetProduct.Include(IncludeInfo => IncludeInfo.IdCategoriaNavigation).First();
                
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveProductInApplication(int idProduct)
        {
            try
            {
                var existingProduct = await this._genericPrincipalRepository.GetSpecificInformation(SearchProduct => 
                SearchProduct.IdProduct == idProduct);

                if(existingProduct == null)
                    throw new TaskCanceledException("El producto no existe");

                string ImageName = existingProduct.NameImage;
                var ResultToOperationRemove = await this._genericPrincipalRepository.DeleteSpecificInformation(existingProduct);
                if (ResultToOperationRemove)
                    await this._servicesFirebaseStorage.RemoveStorage("carpeta_producto", ImageName);

                return true;
            }  
            catch
            {

                throw;
            }
        }
    }
}
