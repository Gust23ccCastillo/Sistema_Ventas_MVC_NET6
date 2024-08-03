using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesBusiness
    {
        Task<Business> GetApplicationBusiness();
        Task<Business> SaveChangesBusiness(Business entity, Stream logo = null, string nameLogo = "");
    }
}
