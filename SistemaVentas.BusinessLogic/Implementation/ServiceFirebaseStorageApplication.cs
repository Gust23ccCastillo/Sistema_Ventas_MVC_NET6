using Firebase.Auth;
using Firebase.Storage;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServiceFirebaseStorageApplication : IServicesFirebaseStorage
    {
        private readonly IGenericPrincipalRepository<ConfigurationApplication> _GenericPrincipalRepository;

        public ServiceFirebaseStorageApplication(IGenericPrincipalRepository<ConfigurationApplication> genericPrincipalRepository)
        {
            _GenericPrincipalRepository = genericPrincipalRepository;
        }

        public async Task<bool> RemoveStorage(string DestinationFolder, string NameArchive)
        {

            try
            {
                //TRAER LA INFORMACION POR MEDIO DEL SERVICIO GENERICO DE EL SERVICIO DE FIREBASE EN LA BD
                IQueryable<ConfigurationApplication> QueryInformation = await this._GenericPrincipalRepository
                    .ConsultSpecificInformation(SearchInfo => SearchInfo.ResourceConfig.Equals("FireBase_Storage"));

                //ALMACENAR LA INFORMACION EN UN DICCIONARIO POR MEDIO DE CLAVE,VALOR
                Dictionary<string, string> StoreInformation = QueryInformation.ToDictionary(keySelector: FirstString => FirstString.Property,
                    elementSelector: SecondString => SecondString.ValueConfig);

                var CreateAuthentication = new FirebaseAuthProvider(new FirebaseConfig(StoreInformation["api_key"]));
                var AuthenticationVerifyCredentials = await CreateAuthentication.SignInWithEmailAndPasswordAsync(StoreInformation["email"], StoreInformation["clave"]);
                if (AuthenticationVerifyCredentials is null || AuthenticationVerifyCredentials == null)
                {
                    throw new Exception("Usuario no Autenticado!!");
                }
                var Cancellation = new CancellationTokenSource();

                var FileDeleteInFireBase = new FirebaseStorage(StoreInformation["ruta"],
                    new FirebaseStorageOptions
                    {
                        //TOKEN DEL USUARIO YA AUTENTICADO EN FIRE BASE
                        AuthTokenAsyncFactory = () => Task.FromResult(AuthenticationVerifyCredentials.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(StoreInformation[DestinationFolder])
                    .Child(NameArchive)
                    .DeleteAsync();

                await FileDeleteInFireBase;// IMAGEN O ARCHIVO ELIMINADO EN FIRE BASE
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> UploadStorage(Stream Archive, string DestinationFolder, string NameArchive)
        {
            string UrlImage = string.Empty;
            try
            {
                //TRAER LA INFORMACION POR MEDIO DEL SERVICIO GENERICO DE EL SERVICIO DE FIREBASE EN LA BD
                IQueryable<ConfigurationApplication> QueryInformation = await this._GenericPrincipalRepository
                    .ConsultSpecificInformation(SearchInfo => SearchInfo.ResourceConfig.Equals("FireBase_Storage"));

                //ALMACENAR LA INFORMACION EN UN DICCIONARIO POR MEDIO DE CLAVE,VALOR
                Dictionary<string, string> StoreInformation = QueryInformation.ToDictionary(keySelector: FirstString => FirstString.Property,
                    elementSelector: SecondString => SecondString.ValueConfig);

                var CreateAuthentication = new FirebaseAuthProvider(new FirebaseConfig(StoreInformation["api_key"]));
                var AuthenticationVerifyCredentials = await CreateAuthentication.SignInWithEmailAndPasswordAsync(StoreInformation["email"], StoreInformation["clave"]);
                if(AuthenticationVerifyCredentials is null || AuthenticationVerifyCredentials == null)
                {
                    throw new Exception("Usuario no Autenticado!!");
                }
                var Cancellation = new CancellationTokenSource();

                var FileCreationInFireBase = new FirebaseStorage(StoreInformation["ruta"],
                    new FirebaseStorageOptions
                    {
                        //TOKEN DEL USUARIO YA AUTENTICADO EN FIRE BASE
                        AuthTokenAsyncFactory = () => Task.FromResult(AuthenticationVerifyCredentials.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(StoreInformation[DestinationFolder])
                    .Child(NameArchive)
                    .PutAsync(Archive, Cancellation.Token);

                UrlImage = await FileCreationInFireBase;//URL DE LA IMAGEN O ARCHIVO CREADO EN FIRE BASE
            }
            catch
            {
                UrlImage = string.Empty;
            }

            return UrlImage;
        }
    }
}
