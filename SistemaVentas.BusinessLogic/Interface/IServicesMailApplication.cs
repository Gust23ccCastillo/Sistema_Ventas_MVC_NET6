namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesMailApplication
    {
        Task<bool> SendMailApplication(string DestinationMail, string Subject, string Message);
    }
}
