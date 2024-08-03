using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesProductApplication
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> CreateProductInApplication(Product product, Stream image = null, string imageName = "");
        Task<Product> EditProductInApplication(Product product, Stream image = null, string imageName = "");

        Task<bool> RemoveProductInApplication(int idProduct);
    }
}
