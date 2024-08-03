using Microsoft.EntityFrameworkCore;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.Dal.Implementation
{
    public class SalesRepository : GenericPrincipalRepository<Sale>, ISalesRepository
    {
        private readonly DataBase_Application_Sales_MVCContext _DbContextApplication;

        public SalesRepository(DataBase_Application_Sales_MVCContext dbContextApplication):base(dbContextApplication)
        {
            _DbContextApplication = dbContextApplication;
        }

        public async Task<Sale> RegisterSale(Sale Entity)
        {
            Sale SaleGenerate = new Sale();
            using (var transaction = this._DbContextApplication.Database.BeginTransaction())
            {
                try
                {
                    //PRIMERO DISMINUIMOS EN STOCK DE EL O LOS PRODUCTOS A COMPRADOS EN NUESTRA DB
                    foreach (SaleDetail saleDetail in Entity.SaleDetails)
                    {
                        Product Found_Product=this._DbContextApplication.Products.
                            Where(SearchProduct => SearchProduct.IdProduct== saleDetail.IdProduct).First();

                        Found_Product.Stock = Found_Product.Stock - saleDetail.Quantity;
                        this._DbContextApplication.Update(Found_Product);
                    }
                    await this._DbContextApplication.SaveChangesAsync();

                    //CREAR EL NUMERO DE CORRELATIVO CORRELATIVO
                    CorrelativeNumber correlativeNumber = this._DbContextApplication.CorrelativeNumbers.Where(
                        SearchInfo => SearchInfo.Gestion == "Realization_Of_Sales").First();

                    correlativeNumber.LastNumber = correlativeNumber.LastNumber + 1;
                    correlativeNumber.DateOfUpdate = DateTime.Now;
                    this._DbContextApplication.CorrelativeNumbers.Update(correlativeNumber);
                    await this._DbContextApplication.SaveChangesAsync();

                    //CREAMOS EL NUMERO DE VENTA CON SUS CEROS
                    string ceros = string.Concat(Enumerable.Repeat("0", correlativeNumber.QuantityDigits.Value));
                    string NumberSale = ceros + correlativeNumber.LastNumber.ToString();
                    NumberSale = NumberSale.Substring(NumberSale.Length - correlativeNumber.QuantityDigits.Value, correlativeNumber.QuantityDigits.Value);
                    Entity.SalesNumber = NumberSale;
                    await this._DbContextApplication.Sales.AddAsync(Entity);
                    await this._DbContextApplication.SaveChangesAsync();

                    SaleGenerate = Entity;
                    transaction.Commit();//REGISTROS NETOS EN NUESTRAS TABLAS DE BASE DATOS.
                }
                catch (Exception ex)
                {
                    //REVERTIR TODO EN LA BASE DE DATOS EN CASO DE FALLO
                    transaction.Rollback();
                    throw ex;
                }

                return SaleGenerate;
            }
        }

        public async Task<List<SaleDetail>> ReportSale(DateTime StartDate, DateTime EndDate)
        {
            List<SaleDetail> _ListSaleDetail = await this._DbContextApplication.SaleDetails
                 .Include(IncludeTable => IncludeTable.IdSaleNavigation)
                 .ThenInclude(Info => Info.IdUserNavigation)
                 .Include(IncludeTable => IncludeTable.IdSaleNavigation)
                 .ThenInclude(Info => Info.IdTypeOfDocumentSaleNavigation)
                 .Where(SearchInfo => SearchInfo.IdSaleNavigation.RegistrationDate.Value >= StartDate.Date &&
                 SearchInfo.IdSaleNavigation.RegistrationDate.Value <= EndDate.Date).ToListAsync();
            return _ListSaleDetail;
        }
    }
}
