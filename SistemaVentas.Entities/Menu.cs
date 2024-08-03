namespace SistemaVentas.Entities
{
    public partial class Menu
    {
        public Menu()
        {
            InverseIdMenuFatherNavigation = new HashSet<Menu>();
            RolMenus = new HashSet<RolMenu>();
        }

        public int IdMenu { get; set; }
        public string? Descriptions { get; set; }
        public int? IdMenuFather { get; set; }
        public string? Icon { get; set; }
        public string? Controller { get; set; }
        public string? ActionPage { get; set; }
        public bool? ItsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual Menu? IdMenuFatherNavigation { get; set; }
        public virtual ICollection<Menu> InverseIdMenuFatherNavigation { get; set; }
        public virtual ICollection<RolMenu> RolMenus { get; set; }
    }
}
