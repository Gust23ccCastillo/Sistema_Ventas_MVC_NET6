using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesSales
    {
        Task<List<Product>> ObtainListProducts(string searchOf);
        Task<Sale> RegisterSaleApplication(Sale saleParameter);
        Task<List<Sale>> SalesHistory(string saleNumber,string starDate, string endDate);
        Task<Sale> GetDetail_Of_A_SpecificSale(string saleNumber);
        Task<List<SaleDetail>> SalesDetailsHistory(string startDate, string endDate);
    }
}
