using Microsoft.EntityFrameworkCore;
using SistemaVentas.Dal.Interface;
using System.Linq.Expressions;

namespace SistemaVentas.Dal.Implementation
{
    public class GenericPrincipalRepository<TEntity> : IGenericPrincipalRepository<TEntity> where TEntity : class
    {
        private readonly DataBase_Application_Sales_MVCContext _DbContextApplication;

        public GenericPrincipalRepository(DataBase_Application_Sales_MVCContext dbContextApplication)
        {
            _DbContextApplication = dbContextApplication;
        }

        public async Task<TEntity> GetSpecificInformation(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                TEntity entity = await this._DbContextApplication.Set<TEntity>().FirstOrDefaultAsync(filter);
                return entity;
            }
            catch
            {
                throw;
            }
        }
       
        public async Task<TEntity> CreateSpecificInformation(TEntity entity)
        {
            try
            {
                this._DbContextApplication.Set<TEntity>().Add(entity);
                await this._DbContextApplication.SaveChangesAsync();
                return entity;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> UpdateSpecificInformation(TEntity entity)
        {
            try
            {
                this._DbContextApplication.Set<TEntity>().Update(entity);
                await this._DbContextApplication.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteSpecificInformation(TEntity entity)
        {
            try
            {
                this._DbContextApplication.Set<TEntity>().Remove(entity);
                await this._DbContextApplication.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TEntity>> ConsultSpecificInformation(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> queryEntity = filter == null ? this._DbContextApplication.Set<TEntity>() : 
                this._DbContextApplication.Set<TEntity>().Where(filter);
            return queryEntity;
        }

    }
}
