using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesRolesApplication
    {
        Task<List<Rol>> ListRolsInApplication();
    }
}
