using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesSaleTypeApplication : IServicesSaleType
    {
        private readonly IGenericPrincipalRepository<TypeOfDocumentSale> _genericPrincipal;

        public ServicesSaleTypeApplication(IGenericPrincipalRepository<TypeOfDocumentSale> genericPrincipal)
        {
            _genericPrincipal = genericPrincipal;
        }

        public async Task<List<TypeOfDocumentSale>> ListOfTypeSales()
        {
            IQueryable<TypeOfDocumentSale> queryList = await this._genericPrincipal.ConsultSpecificInformation();
            return queryList.ToList();
        }
    }
}
