using System;
using System.Collections.Generic;

namespace SistemaVentas.Entities
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int IdCategoria { get; set; }
        public string? Descriptions { get; set; }
        public bool? ItsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
