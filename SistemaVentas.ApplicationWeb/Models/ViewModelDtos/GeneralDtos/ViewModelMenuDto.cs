namespace SistemaVentas.ApplicationWeb.Models.ViewModelDtos.GeneralDtos
{
    public class ViewModelMenuDto
    {
        public string? Descriptions { get; set; }
        public string? Icon { get; set; }
        public string? Controller { get; set; }
        public string? ActionPage { get; set; }
        public virtual ICollection<ViewModelMenuDto> SubMenus { get; set; }

    }
}
