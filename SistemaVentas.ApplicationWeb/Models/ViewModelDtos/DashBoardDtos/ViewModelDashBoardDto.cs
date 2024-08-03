using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.ProductDtos;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos;

namespace SistemaVentas.ApplicationWeb.Models.ViewModelDtos.DashBoardDtos
{
    public class ViewModelDashBoardDto
    {

        public int TotalSales { get; set; }
        //INGRESOS TOTALES
        public string? TotalRevenues { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategorys { get; set; }
        public List<ViewModelWeekendSalesDto> WeekendSales { get; set; }
        public List<ViewModelProductsByWeekDto> ProductsByWeek { get; set; }
    }
}
