namespace SistemaVentas.ApplicationWeb.Models.ViewModelDtos.User_Password_LoginDtos
{
    public class ViewModelUserDto
    {

        public int IdUser { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? IdRol { get; set; }
        public string? NameRol { get; set; }
        public string? UrlPhoto { get; set; }
        public int? ItsActive { get; set; }

    }
}
