using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesSaleType
    {
        Task<List<TypeOfDocumentSale>> ListOfTypeSales();
    }
}
