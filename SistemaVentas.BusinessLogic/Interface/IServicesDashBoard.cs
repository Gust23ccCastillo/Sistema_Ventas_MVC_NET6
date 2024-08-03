namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesDashBoard
    {
        Task<int> TotalSalesLastWeek();
        Task<string> TotalSalesRevenueLastWeek();
        Task<int> TotalProductsInApplication();
        Task<int> TotalCategorysInApplication();

        Task<Dictionary<string,int>> LastWeekSalesDataGraph();
        Task<Dictionary<string, int>> TopSellingProductsChartDataLastWeek();
    }
}
