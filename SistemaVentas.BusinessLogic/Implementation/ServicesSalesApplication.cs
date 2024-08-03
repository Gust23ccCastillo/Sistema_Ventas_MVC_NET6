using Microsoft.EntityFrameworkCore;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;
using System.Dynamic;
using System.Globalization;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesSalesApplication: IServicesSales
    {
        private readonly IGenericPrincipalRepository<Product> _productRepository;
        private readonly ISalesRepository _salesRepository;

        public ServicesSalesApplication(IGenericPrincipalRepository<Product> productRepository, 
            ISalesRepository salesRepository)
        {
            _productRepository = productRepository;
            _salesRepository = salesRepository;
        }

        public async Task<List<Product>> ObtainListProducts(string searchOf)
        {
            IQueryable<Product> ListProductSearch = await this._productRepository.ConsultSpecificInformation(
                SearchInfo => SearchInfo.ItsActive == true &&
                SearchInfo.Stock > 0 &&
                string.Concat(SearchInfo.Barcode, SearchInfo.Mark, SearchInfo.Descriptions).Contains(searchOf));
            return ListProductSearch.Include(AgregateCategoryInfo=>AgregateCategoryInfo.IdCategoriaNavigation).ToList();

        }
        public async Task<Sale> RegisterSaleApplication(Sale saleParameter)
        {
            try
            {
                return await this._salesRepository.RegisterSale(saleParameter);
            }
            catch
            {

                throw;
            }
        }

        public async Task<List<Sale>> SalesHistory(string saleNumber, string starDate, string endDate)
        {
            IQueryable<Sale> GetData_ForSales_With_SpecificDates = await this._salesRepository.ConsultSpecificInformation();
            starDate = starDate is null ? "" : starDate;
            endDate = endDate is null ? "" : endDate;

            //BUSQUEDA POR RANGO DE FECHAS
            if(starDate != "" && endDate != "")
            {
                DateTime ConverToDateTimeStartDate = Convert.ToDateTime(starDate);
                DateTime ConverToDateTimeEndDate = Convert.ToDateTime(endDate);
                return GetData_ForSales_With_SpecificDates.Where(SearchSales =>
                SearchSales.RegistrationDate >= ConverToDateTimeStartDate &&
                SearchSales.RegistrationDate <= ConverToDateTimeEndDate
                )
                .Include(Include_Info_TypeDocument => Include_Info_TypeDocument.IdTypeOfDocumentSaleNavigation)
                .Include(Include_Info_User => Include_Info_User.IdUserNavigation)
                .Include(Include_Info_DetailSale => Include_Info_DetailSale.SaleDetails)
                .ToList();

            }
            //BUSQUEDA POR NUMERO DE VENTA
            else
            {
                var List = GetData_ForSales_With_SpecificDates.Where(search => search.SalesNumber == saleNumber)
               .Include(Include_Info_TypeDocument => Include_Info_TypeDocument.IdTypeOfDocumentSaleNavigation)
               .Include(Include_Info_User => Include_Info_User.IdUserNavigation)
               .Include(Include_Info_DetailSale => Include_Info_DetailSale.SaleDetails)
               .ToList();

                //Console.WriteLine(List);
                return List;
            }
            
            
        }

        public async Task<Sale> GetDetail_Of_A_SpecificSale(string saleNumber)
        {
            IQueryable<Sale> GetData_ForSales_With_SpecificDates = await this._salesRepository.ConsultSpecificInformation(SearhInformation =>
            SearhInformation.SalesNumber == saleNumber);
            return GetData_ForSales_With_SpecificDates
              .Include(Include_Info_TypeDocument => Include_Info_TypeDocument.IdTypeOfDocumentSaleNavigation)
              .Include(Include_Info_User => Include_Info_User.IdUserNavigation)
              .Include(Include_Info_DetailSale => Include_Info_DetailSale.SaleDetails)
              .First();
        }

        public async Task<List<SaleDetail>> SalesDetailsHistory(string startDate, string endDate)
        {
            List<SaleDetail> ListSaleDetail = await this._salesRepository.ReportSale(
                DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-CR")),
                DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-CR")));
            return ListSaleDetail;
             
        }

       
    }
}
