using static System.Net.Mime.MediaTypeNames;

namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesProfits
    {
        string GeneratePassword();
        string PasswordEncryption(string ParameterText);
    }
}
