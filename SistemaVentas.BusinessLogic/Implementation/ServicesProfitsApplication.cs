using SistemaVentas.BusinessLogic.Interface;
using System.Security.Cryptography;
using System.Text;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesProfitsApplication : IServicesProfits
    {
        public string GeneratePassword()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }

        public string PasswordEncryption(string ParameterText)
        {
           StringBuilder stringBuilder = new StringBuilder();
            using(SHA256 Hash = SHA256Managed.Create()) 
            {
                Encoding encoding = Encoding.UTF8;
                byte[] result = Hash.ComputeHash(encoding.GetBytes(ParameterText));
                foreach (byte read in result)
                {
                    stringBuilder.Append(read.ToString("x2"));
                }
            }

            return stringBuilder.ToString();
        }
    }
}
