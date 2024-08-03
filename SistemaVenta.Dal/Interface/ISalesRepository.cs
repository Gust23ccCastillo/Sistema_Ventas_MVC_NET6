using SistemaVentas.Entities;

namespace SistemaVentas.Dal.Interface
{
    public interface ISalesRepository:IGenericPrincipalRepository<Sale>
    {
        Task<Sale> RegisterSale(Sale Entity);
        Task<List<SaleDetail>> ReportSale(DateTime StartDate, DateTime EndDate);
    }
}
