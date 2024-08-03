using Microsoft.VisualBasic;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;
using System.Net;
using System.Net.Mail;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesMailApplication : IServicesMailApplication
    {
        private readonly IGenericPrincipalRepository<ConfigurationApplication> _RepositoryConfigurationApplication;

        public ServicesMailApplication(IGenericPrincipalRepository<ConfigurationApplication> repositoryConfigurationApplication)
        {
            _RepositoryConfigurationApplication = repositoryConfigurationApplication;
        }

        public async Task<bool> SendMailApplication(string DestinationMail, string Subject, string Message)
        {
            try
            {
                //TRAER LA INFORMACION POR MEDIO DEL SERVICIO GENERICO DE EL SERVICIO DE CORREO EN LA BD
                IQueryable<ConfigurationApplication> QueryInformation = await this._RepositoryConfigurationApplication
                    .ConsultSpecificInformation(SearchInfo => SearchInfo.ResourceConfig.Equals("Servicio_Correo"));

                //ALMACENAR LA INFORMACION EN UN DICCIONARIO POR MEDIO DE CLAVE,VALOR
                Dictionary<string, string> StoreInformation = QueryInformation.ToDictionary(keySelector: FirstString=>FirstString.Property,
                    elementSelector: SecondString=>SecondString.ValueConfig );


                //CREAR EL EMAIL Y SUS CREDENCIALES
                var Credentials = new NetworkCredential(StoreInformation["correo"], StoreInformation["clave"]);
                var NewEmail = new MailMessage()
                {
                    From = new MailAddress(StoreInformation["correo"], StoreInformation["alias"]),
                    Subject = Subject,
                    Body = Message,
                    IsBodyHtml = true,
                };
                NewEmail.To.Add(new MailAddress(DestinationMail));

                //CONFIGURAR EL CLIENTE SERVIDOR
                var ClientServer = new SmtpClient()
                {
                    Host = StoreInformation["host"],
                    Credentials = Credentials,
                    Port = int.Parse(StoreInformation["puerto"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                };

                ClientServer.Send(NewEmail);
                return true;
            }
            catch
            {
                return false;
               
            }
        }
    }
}
