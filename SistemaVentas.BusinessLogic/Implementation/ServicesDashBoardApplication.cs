using Microsoft.EntityFrameworkCore;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;
using System.Globalization;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesDashBoardApplication : IServicesDashBoard
    {
        private readonly ISalesRepository _salesRepositoryInject;
        private readonly IGenericPrincipalRepository<SaleDetail> _saleDetailInject;
        private readonly IGenericPrincipalRepository<Category> _categoryInject;
        private readonly IGenericPrincipalRepository<Product> _productInject;
        private DateTime _startDate = DateTime.Now;

        public ServicesDashBoardApplication(ISalesRepository salesRepositoryInject, 
            IGenericPrincipalRepository<SaleDetail> saleDetailInject, 
            IGenericPrincipalRepository<Category> categoryInject, 
            IGenericPrincipalRepository<Product> productInject, 
            DateTime startDate)
        {
            _salesRepositoryInject = salesRepositoryInject;
            _saleDetailInject = saleDetailInject;
            _categoryInject = categoryInject;
            _productInject = productInject;
            _startDate = startDate.AddDays(-7);
        }

        public async Task<int> TotalSalesLastWeek()
        {
            try
            {
                IQueryable<Sale> saleInfo = await this._salesRepositoryInject.ConsultSpecificInformation(
                    getSaleInLastWeek => getSaleInLastWeek.RegistrationDate.Value.Date >= this._startDate.Date);
                int totalSaleInLastWeek=saleInfo.Count();
                return totalSaleInLastWeek;
            }
            catch
            {

                throw;
            }
        }
        public async Task<string> TotalSalesRevenueLastWeek()
        {
            try
            {
                IQueryable<Sale> saleInfo = await this._salesRepositoryInject.ConsultSpecificInformation(
                   getSaleInLastWeek => getSaleInLastWeek.RegistrationDate.Value.Date >= this._startDate.Date);

                decimal resultTotalSaleRevenueLastWeek = saleInfo.Select(selectInfo => selectInfo.Total)
                    .Sum(selectInfo => selectInfo.Value);

                return Convert.ToString(resultTotalSaleRevenueLastWeek, new CultureInfo("es-CR"));
            }
            catch
            {

                throw;
            }
        }

        public async Task<int> TotalProductsInApplication()
        {

            IQueryable<Product> productsInfo = await this._productInject.ConsultSpecificInformation();
            int totalProductInLastWeek = productsInfo.Count();
            return totalProductInLastWeek;
        }

        public async Task<int> TotalCategorysInApplication()
        {

            IQueryable<Category> categorysInfo = await this._categoryInject.ConsultSpecificInformation();
            int totalCategorysInLastWeek = categorysInfo.Count();
            return totalCategorysInLastWeek;
        }

        public async Task<Dictionary<string, int>> LastWeekSalesDataGraph()
        {
            try
            {
                IQueryable<Sale> salesRegistersInSystem = await this._salesRepositoryInject.ConsultSpecificInformation(
                    getInfo => getInfo.RegistrationDate.Value.Date >= this._startDate.Date);

                Dictionary<string, int> RecordDatesAndTotalSales = salesRegistersInSystem.GroupBy(_groupBy
                    => _groupBy.RegistrationDate.Value.Date).OrderByDescending(_orderBy => _orderBy.Key)
                    .Select(selectInfoBy => new { Fecha = selectInfoBy.Key.ToString("dd/MM/yyyy"), TotalSales = selectInfoBy.Count() })
                    .ToDictionary(keySelector: selectKey => selectKey.Fecha, elementSelector: selectValue => selectValue.TotalSales);

                return RecordDatesAndTotalSales;
            }
            catch
            {

                throw;
            }
        }

        public async Task<Dictionary<string, int>> TopSellingProductsChartDataLastWeek()
        {

            try
            {
                IQueryable<SaleDetail> saleDetailsRegistersInSystem = await this._saleDetailInject.ConsultSpecificInformation();

                Dictionary<string, int> BestSellingProductsInSystem = saleDetailsRegistersInSystem
                    .Include(includeSale => includeSale.IdSaleNavigation)
                    .Where(search => search.IdSaleNavigation.RegistrationDate.Value.Date >= _startDate.Date)
                    .GroupBy(groupByProductDescription => groupByProductDescription.ProductDescription)
                    .OrderByDescending(orderByQuantity => orderByQuantity.Count())
                    .Select(select => new {Producto = select.Key, Total = select.Count()}).Take(4)
                    .ToDictionary(keySelector: selectKey => selectKey.Producto, elementSelector: selectValue => selectValue.Total);
                
                  

                return BestSellingProductsInSystem;
            }
            catch
            {

                throw;
            }
        }

    }
}
