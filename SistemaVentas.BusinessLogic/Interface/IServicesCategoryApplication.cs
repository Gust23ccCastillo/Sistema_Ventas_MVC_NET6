using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesCategoryApplication
    {
        Task<List<Category>> GetListApplicationCategories();
        Task<Category> CreateCategoryApplication(Category categoryEntity);
        Task<Category> UpdateCategoryApplication(Category categoryUpdatedInformation);
        Task<bool> RemoveCategoryApplication(int idCategory);
    }
}
