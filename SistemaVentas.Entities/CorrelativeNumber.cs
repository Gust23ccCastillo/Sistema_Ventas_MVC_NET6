using System;
using System.Collections.Generic;

namespace SistemaVentas.Entities
{
    public partial class CorrelativeNumber
    {
        public int IdCorrelativeNumber { get; set; }
        public int? LastNumber { get; set; }
        public int? QuantityDigits { get; set; }
        public string? Gestion { get; set; }
        public DateTime? DateOfUpdate { get; set; }
    }
}
