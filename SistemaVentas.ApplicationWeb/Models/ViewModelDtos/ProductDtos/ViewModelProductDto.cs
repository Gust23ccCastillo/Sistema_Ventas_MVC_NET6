namespace SistemaVentas.ApplicationWeb.Models.ViewModelDtos.ProductDtos
{
    public class ViewModelProductDto
    {
        public int IdProduct { get; set; }
        public string? Barcode { get; set; }
        public string? Mark { get; set; }
        public string? Descriptions { get; set; }
        public int? IdCategoria { get; set; }
        public string? NameCategory { get; set; }
        public int? Stock { get; set; }
        public string? UrlImage { get; set; }
        public string? Price { get; set; }
        public int? ItsActive { get; set; }
    }
}
