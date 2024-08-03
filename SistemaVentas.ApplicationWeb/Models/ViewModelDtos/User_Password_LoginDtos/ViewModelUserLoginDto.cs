namespace SistemaVentas.ApplicationWeb.Models.ViewModelDtos.User_Password_LoginDtos
{
    public class ViewModelUserLoginDto
    {
        public string? MailOfUser { get; set; }
        public string? PasswordOfUser { get; set; }
        public bool MaintainUserSession { get; set; }
    }
}
