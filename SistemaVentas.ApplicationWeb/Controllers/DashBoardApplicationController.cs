using Microsoft.AspNetCore.Mvc;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.DashBoardDtos;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.ProductDtos;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos;
using SistemaVentas.ApplicationWeb.Profits.Response;
using SistemaVentas.BusinessLogic.Interface;

namespace SistemaVentas.ApplicationWeb.Controllers
{
    public class DashBoardApplicationController : Controller
    {
        private readonly IServicesDashBoard servicesDashBoardInject;

        public DashBoardApplicationController(IServicesDashBoard servicesDashBoardInject)
        {
            this.servicesDashBoardInject = servicesDashBoardInject;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GeneralDataForTheDashBoard()
        {
            GenericResponse<ViewModelDashBoardDto> responseController = new();
            try
            {
                ViewModelDashBoardDto ModelDashBoard = new();
                ModelDashBoard.TotalSales = await this.servicesDashBoardInject.TotalSalesLastWeek();
                ModelDashBoard.TotalRevenues = await this.servicesDashBoardInject.TotalSalesRevenueLastWeek();
                ModelDashBoard.TotalProducts = await this.servicesDashBoardInject.TotalProductsInApplication();
                ModelDashBoard.TotalCategorys = await this.servicesDashBoardInject.TotalCategorysInApplication();

                List<ViewModelWeekendSalesDto> ListWeekendSales = new ();
                List<ViewModelProductsByWeekDto> ListProductsByWeek = new ();

                //RECORRER Y AGRUPAR EN LA LISTA LOS DATOS DE LAS VENTAS EN LA ULTIMA SEMANA POR FECHA Y SU TOTAL 
                foreach (KeyValuePair<string,int> item in await this.servicesDashBoardInject.LastWeekSalesDataGraph())
                {
                    ListWeekendSales.Add(new ViewModelWeekendSalesDto()
                    {
                        Date = item.Key,
                        Total = item.Value
                    });
                }

                //RECORRER Y AGRUPAR EN LA LISTA LOS DATOS DE LOS PRUCTOS EN LA ULTIMA SEMANA MAS VENDIDOS POR PRODUCTOS Y SU CANTIDAD 
                foreach (KeyValuePair<string, int> item in await this.servicesDashBoardInject.TopSellingProductsChartDataLastWeek())
                {
                    ListProductsByWeek.Add(new ViewModelProductsByWeekDto()
                    {
                        Product = item.Key,
                        Quantity = item.Value
                    });
                }

                ModelDashBoard.WeekendSales = ListWeekendSales;
                ModelDashBoard.ProductsByWeek = ListProductsByWeek;

                responseController.State = true;
                responseController.Object = ModelDashBoard;

            }
            catch (Exception ex)
            {

                responseController.State = false;
                responseController.Message = "Problemas con el servidor para mostrar los datos!!!" + ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, responseController);
        }


    }
}
