using System;
using System.Collections.Generic;

namespace SistemaVentas.Entities
{
    public partial class User
    {
        public User()
        {
            Sales = new HashSet<Sale>();
        }

        public int IdUser { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? IdRol { get; set; }
        public string? UrlPhoto { get; set; }
        public string? NamePhoto { get; set; }
        public string? UserPassword { get; set; }
        public bool? ItsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
