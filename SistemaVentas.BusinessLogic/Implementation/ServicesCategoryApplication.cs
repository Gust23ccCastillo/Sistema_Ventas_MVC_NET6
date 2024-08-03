using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesCategoryApplication : IServicesCategoryApplication
    {
        private readonly IGenericPrincipalRepository<Category> _genericPrincipalRepository;

        public ServicesCategoryApplication(IGenericPrincipalRepository<Category> genericPrincipalRepository)
        {
            _genericPrincipalRepository = genericPrincipalRepository;
        }
        public async Task<List<Category>> GetListApplicationCategories()
        {
            IQueryable<Category> GetListGategory = await this._genericPrincipalRepository.ConsultSpecificInformation();
            return GetListGategory.ToList();
        }

        public async Task<Category> CreateCategoryApplication(Category categoryEntity)
        {
            try
            {
                var CreateCategoryResult = await this._genericPrincipalRepository.CreateSpecificInformation(categoryEntity);
                if(categoryEntity.IdCategoria == 0)
                {
                    throw new TaskCanceledException("No se pudo crear la categoria");
                }

                return CreateCategoryResult;
            }
            catch
            {

                throw;
            }
        }

        public async Task<Category> UpdateCategoryApplication(Category categoryUpdatedInformation)
        {
            try
            {
                var GetSpecificCategory = await this._genericPrincipalRepository.GetSpecificInformation(
                    searchInformation => searchInformation.IdCategoria == categoryUpdatedInformation.IdCategoria);

                GetSpecificCategory.Descriptions = categoryUpdatedInformation.Descriptions;
                GetSpecificCategory.ItsActive = categoryUpdatedInformation.ItsActive;

                var ResultToOperation = await this._genericPrincipalRepository.UpdateSpecificInformation(GetSpecificCategory);
                if (!ResultToOperation)
                    throw new TaskCanceledException("No se pudo modificar la categoria");

                return GetSpecificCategory;
            }
            catch
            {

                throw;
            }
        }

        public async Task<bool> RemoveCategoryApplication(int idCategory)
        {
            try
            {
                var GetSpecificCategory = await this._genericPrincipalRepository.GetSpecificInformation(
                 searchInformation => searchInformation.IdCategoria == idCategory);

                if(GetSpecificCategory == null)
                    throw new TaskCanceledException("La Categoria no existe");

                var ResultToOperation = await this._genericPrincipalRepository.DeleteSpecificInformation(GetSpecificCategory);
                return ResultToOperation;
            }
            catch 
            {

                throw;
            }
        }

       
    }
}
