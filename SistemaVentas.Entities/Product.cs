using System;
using System.Collections.Generic;

namespace SistemaVentas.Entities
{
    public partial class Product
    {
        public int IdProduct { get; set; }
        public string? Barcode { get; set; }
        public string? Mark { get; set; }
        public string? Descriptions { get; set; }
        public int? IdCategoria { get; set; }
        public int? Stock { get; set; }
        public string? UrlImage { get; set; }
        public string? NameImage { get; set; }
        public decimal? Price { get; set; }
        public bool? ItsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual Category? IdCategoriaNavigation { get; set; }
    }
}
