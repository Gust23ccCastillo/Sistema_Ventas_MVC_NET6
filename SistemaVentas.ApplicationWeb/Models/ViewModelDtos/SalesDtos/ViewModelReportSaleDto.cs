namespace SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos
{
    public class ViewModelReportSaleDto
    {
        public string? RegistrationDate { get; set; }
        public string? NumberSale { get; set; }
        public string? TypeOfDocument { get; set; }
        public string? ClientDocument { get; set; }
        public string? ClientName { get; set; }
        public string? SubTotal { get; set; }
        public string? TotalTax { get; set; }
        public string? TotalSale { get; set; }
        public string? Product { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string? Price { get; set; }
        public string? Total { get; set; }
    }
}
