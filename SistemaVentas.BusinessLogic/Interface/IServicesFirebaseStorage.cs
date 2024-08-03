namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesFirebaseStorage
    {
        Task<string> UploadStorage(Stream Archive, string DestinationFolder,string NameArchive);
        Task<bool>  RemoveStorage(string DestinationFolder, string NameArchive);

    }
}
