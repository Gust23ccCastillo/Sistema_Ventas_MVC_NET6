using System;
using System.Collections.Generic;

namespace SistemaVentas.Entities
{
    public partial class Business
    {
        public int IdBusiness { get; set; }
        public string? UrlLogo { get; set; }
        public string? LogoName { get; set; }
        public string? DocumentNumber { get; set; }
        public string? NameBusiness { get; set; }
        public string? Email { get; set; }
        public string? AddressBusiness { get; set; }
        public string? Phone { get; set; }
        public decimal? PercentageTax { get; set; }
        public string? Coin { get; set; }
    }
}
