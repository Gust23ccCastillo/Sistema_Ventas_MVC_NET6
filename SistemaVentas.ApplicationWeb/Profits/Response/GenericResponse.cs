namespace SistemaVentas.ApplicationWeb.Profits.Response
{
    //CLASE GENERICA DE RESPUESTA A TODAS LA SOLICITUDES A NUESTRA WEB
    public class GenericResponse<TObject>
    {
        public bool State { get; set; }
        public string? Message { get; set; }
        public TObject? Object { get; set; }
        public List<TObject>? ListObject { get; set; }
    }
}
