using System.Linq.Expressions;

namespace SistemaVentas.Dal.Interface
{
    public interface IGenericPrincipalRepository<TEntity> where TEntity:class
    {
        Task<TEntity> GetSpecificInformation(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> CreateSpecificInformation(TEntity entity);
        Task<bool> UpdateSpecificInformation(TEntity entity);
        Task<bool> DeleteSpecificInformation(TEntity entity);
        Task<IQueryable<TEntity>> ConsultSpecificInformation(Expression<Func<TEntity, bool>> filter = null);
    }
}
