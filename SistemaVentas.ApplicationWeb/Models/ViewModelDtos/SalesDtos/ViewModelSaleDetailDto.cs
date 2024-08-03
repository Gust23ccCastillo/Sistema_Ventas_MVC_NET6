namespace SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos
{
    public class ViewModelSaleDetailDto
    {
        public int? IdProduct { get; set; }
        public string? ProductMark { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
        public int? Quantity { get; set; }
        public string? Price { get; set; }
        public string? Total { get; set; }
       
    }
}
