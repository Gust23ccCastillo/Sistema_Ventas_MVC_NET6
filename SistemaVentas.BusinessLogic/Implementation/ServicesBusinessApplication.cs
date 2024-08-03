using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesBusinessApplication : IServicesBusiness
    {
        private readonly IGenericPrincipalRepository<Business> _principalRepository;
        private readonly IServicesFirebaseStorage _servicesFirebaseStorage;

        public ServicesBusinessApplication(IGenericPrincipalRepository<Business> principalRepository,
            IServicesFirebaseStorage servicesFirebaseStorage)
        {
            _principalRepository = principalRepository;
            _servicesFirebaseStorage = servicesFirebaseStorage;
        }

        public async Task<Business> GetApplicationBusiness()
        {
            try
            {
                //ESTA APLICACION SOLO MANEJA UN UNICO NEGOCIO
                int UniqueBusiness = 1;
                return await this._principalRepository.GetSpecificInformation(SearchInfo =>
                SearchInfo.IdBusiness == UniqueBusiness);
                
            }
            catch
            {

                throw;
            }
        }

        public async Task<Business> SaveChangesBusiness(Business entity, Stream logo = null, string nameLogo = "")
        {
            try
            {
                //ESTA APLICACION SOLO MANEJA UN UNICO NEGOCIO
                int UniqueBusiness = 1;
                Business businessFound = await this._principalRepository.GetSpecificInformation(SearchInfo => 
                SearchInfo.IdBusiness == UniqueBusiness);

                if (businessFound.LogoName == string.Empty && businessFound.UrlLogo == string.Empty &&
                    logo != Stream.Null && nameLogo!= string.Empty)
                {
                    businessFound.UrlLogo = await this._servicesFirebaseStorage.UploadStorage(logo, "carpeta_logo", nameLogo);
                    businessFound.LogoName = nameLogo;
                }
                else if (businessFound.LogoName != string.Empty && businessFound.UrlLogo != string.Empty &&
                     logo != Stream.Null && nameLogo != string.Empty)
                {
                    await this._servicesFirebaseStorage.RemoveStorage("carpeta_logo", businessFound.LogoName);
                    businessFound.UrlLogo = await this._servicesFirebaseStorage.UploadStorage(logo, "carpeta_logo", nameLogo);
                    businessFound.LogoName = nameLogo;
                }

                businessFound.DocumentNumber = entity.DocumentNumber;
                businessFound.NameBusiness = entity.NameBusiness;
                businessFound.Email = entity.Email;
                businessFound.AddressBusiness = entity.AddressBusiness;
                businessFound.Phone = entity.Phone;
                businessFound.PercentageTax = entity.PercentageTax;
                businessFound.Coin = entity.Coin;
               // businessFound.LogoName = businessFound.LogoName == "" ? nameLogo : businessFound.LogoName;

                await this._principalRepository.UpdateSpecificInformation(businessFound);
                return businessFound;
            }
            catch
            {

                throw;
            }
        }
    }
}
