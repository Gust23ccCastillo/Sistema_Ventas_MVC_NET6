using System;
using System.Collections.Generic;

namespace SistemaVentas.Entities
{
    public partial class Rol
    {
        public Rol()
        {
            RolMenus = new HashSet<RolMenu>();
            Users = new HashSet<User>();
        }

        public int IdRol { get; set; }
        public string? Descriptions { get; set; }
        public bool? ItsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<RolMenu> RolMenus { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
