namespace SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos
{
    public class ViewModelSaleDto
    {
        public int IdSale { get; set; }
        public string? SalesNumber { get; set; }
        public int? IdTypeOfDocumentSale { get; set; }
        public string? TypeOfoDocumentSale { get; set; }
        public int? IdUser { get; set; }
        public string? UserProfile { get; set; }
        public string? ClientDocument { get; set; }
        public string? ClientName { get; set; }
        public string? SubTotal { get; set; }
        public string? TotalTax { get; set; }
        public string? Total { get; set; }
        public string? RegistrationDate { get; set; }
        public ICollection<ViewModelSaleDetailDto> SaleDetails { get; set; }
    }
}
