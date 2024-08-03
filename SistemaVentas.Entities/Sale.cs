using System;
using System.Collections.Generic;

namespace SistemaVentas.Entities
{
    public partial class Sale
    {
        public Sale()
        {
            SaleDetails = new HashSet<SaleDetail>();
        }

        public int IdSale { get; set; }
        public string? SalesNumber { get; set; }
        public int? IdTypeOfDocumentSale { get; set; }
        public int? IdUser { get; set; }
        public string? ClientDocument { get; set; }
        public string? ClientName { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Total { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual TypeOfDocumentSale? IdTypeOfDocumentSaleNavigation { get; set; }
        public virtual User? IdUserNavigation { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
