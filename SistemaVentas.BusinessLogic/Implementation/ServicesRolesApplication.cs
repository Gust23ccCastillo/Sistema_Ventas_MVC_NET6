using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;
using System.Data;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesRolesApplication : IServicesRolesApplication
    {
        private readonly IGenericPrincipalRepository<Rol> _GenericRepository;

        public ServicesRolesApplication(IGenericPrincipalRepository<Rol> genericRepository)
        {
            _GenericRepository = genericRepository;
        }

        public async Task<List<Rol>> ListRolsInApplication()
        {
            IQueryable<Rol> ConsultRoleList = await this._GenericRepository.ConsultSpecificInformation();
            return ConsultRoleList.ToList();    
        }
    }
}
