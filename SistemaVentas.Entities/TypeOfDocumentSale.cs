using System;
using System.Collections.Generic;

namespace SistemaVentas.Entities
{
    public partial class TypeOfDocumentSale
    {
        public TypeOfDocumentSale()
        {
            Sales = new HashSet<Sale>();
        }

        public int IdTypeOfDocumentSale { get; set; }
        public string? Descriptions { get; set; }
        public bool? ItsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
