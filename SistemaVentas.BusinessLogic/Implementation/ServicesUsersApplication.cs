
using Microsoft.EntityFrameworkCore;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;
using System.Net;
using System.Text;

namespace SistemaVentas.BusinessLogic.Implementation
{
    public class ServicesUsersApplication : IServicesUsers
    {
        private readonly IGenericPrincipalRepository<User> _GenericPrincipalRepository;
        private readonly IServicesFirebaseStorage _FirebaseStorageRepository;
        private readonly IServicesProfits _ServicesProfitsRepository;
        private readonly IServicesMailApplication _ServicesMailRepository;

        public ServicesUsersApplication(IGenericPrincipalRepository<User> genericPrincipalRepository,
            IServicesFirebaseStorage firebaseStorageRepository, 
            IServicesProfits servicesProfitsRepository, 
            IServicesMailApplication servicesMailRepository)
        {
            _GenericPrincipalRepository = genericPrincipalRepository;
            _FirebaseStorageRepository = firebaseStorageRepository;
            _ServicesProfitsRepository = servicesProfitsRepository;
            _ServicesMailRepository = servicesMailRepository;
        }

        public async Task<List<User>> GetListUsersInApplication()
        {
            IQueryable<User> GetListUser = await this._GenericPrincipalRepository.ConsultSpecificInformation();
            return GetListUser.Include(GetRolInfo => GetRolInfo.IdRolNavigation).ToList();
        }

        public async Task<User> CreateUserInApplication(User UserParameter, Stream UserPhotoParameter = null, string NamePhotoParameter = "", string UrlMailTemplate = "")
        {
            var CheckIfEmailExists = await this._GenericPrincipalRepository.GetSpecificInformation(SearchEmailUserInDatabase =>
            SearchEmailUserInDatabase.Email == UserParameter.Email);
            if(CheckIfEmailExists != null)
            {
                throw new TaskCanceledException("El Correo del Usuario ya se encuentra Registrado!!.");
            }

            try
            {
                string TemporarilyGeneratedPassword = this._ServicesProfitsRepository.GeneratePassword();
                UserParameter.UserPassword = this._ServicesProfitsRepository.PasswordEncryption(TemporarilyGeneratedPassword);
                UserParameter.NamePhoto = NamePhotoParameter;
                if(UserPhotoParameter != Stream.Null)
                {
                    string urlPhoto = await this._FirebaseStorageRepository.UploadStorage(UserPhotoParameter, "carpeta_usuario", NamePhotoParameter);
                    UserParameter.UrlPhoto = urlPhoto;
                }
                else
                {
                    throw new TaskCanceledException("El Usuario para ser 'Agregado' debe incluir una una Foto de Perfil!!!");
                }



                CheckIfEmailExists = await this._GenericPrincipalRepository.CreateSpecificInformation(UserParameter);
                if(CheckIfEmailExists.IdUser == 0) 
                {
                    throw new TaskCanceledException("El Usuario no pudo ser 'CREADO'!!");
                }

                if(UrlMailTemplate != string.Empty || UrlMailTemplate != null)
                {
                    UrlMailTemplate = UrlMailTemplate.Replace("[email]", CheckIfEmailExists.Email).Replace("[password]", TemporarilyGeneratedPassword);
                    string htmlMail = "";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlMailTemplate);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if(response.StatusCode == HttpStatusCode.OK) 
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader streamReader = null;
                            if(response.CharacterSet == null)
                            {
                                streamReader = new StreamReader(dataStream);
                            }
                            else
                            {
                                streamReader = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            htmlMail = streamReader.ReadToEnd();
                            response.Close();
                            streamReader.Close();
                        }
                       
                    }
                    if (htmlMail != string.Empty || htmlMail != null)
                    {
                        await this._ServicesMailRepository.SendMailApplication(CheckIfEmailExists.Email, "Cuenta de Usuario Creada", htmlMail);
                    }
                }
                IQueryable<User> GetInformationUserCreate = await this._GenericPrincipalRepository.ConsultSpecificInformation(GetInfo => GetInfo.IdUser ==
                CheckIfEmailExists.IdUser);
                CheckIfEmailExists = GetInformationUserCreate.Include(RolIncludeInfo => RolIncludeInfo.IdRolNavigation).First();
                return CheckIfEmailExists;
            }
            catch(Exception ex) 
            {
                throw;
            }
        }
        public async Task<User> UpdateUserInApplication(User UserParameter, Stream UserPhotoParameter = null, string NamePhotoParameter = "")
        {
           
            try
            {
                var CheckIfUserExists = await this._GenericPrincipalRepository.GetSpecificInformation(SearchUserInDatabase =>
                   SearchUserInDatabase.Email == UserParameter.Email && SearchUserInDatabase.IdUser != UserParameter.IdUser);

                if (CheckIfUserExists != null)
                {
                    throw new TaskCanceledException("El Correo del Usuario ya se encuentra Registrado!!.");
                }
                IQueryable<User> GetUserInformationInDatabase = await this._GenericPrincipalRepository.ConsultSpecificInformation(SearchInfo => SearchInfo.IdUser == UserParameter.IdUser);
                User User_Update = GetUserInformationInDatabase.First();
               
                

                if(UserPhotoParameter != null && NamePhotoParameter != string.Empty)
                {
                     await this._FirebaseStorageRepository.RemoveStorage("carpeta_usuario", User_Update.NamePhoto);
                     User_Update.NamePhoto = NamePhotoParameter;
                     User_Update.UrlPhoto = await this._FirebaseStorageRepository.UploadStorage(UserPhotoParameter, "carpeta_usuario", User_Update.NamePhoto);
                }
                
                User_Update.UserName = UserParameter.UserName;
                User_Update.Email = UserParameter.Email;
                User_Update.Phone = UserParameter.Phone;
                User_Update.IdRol = UserParameter.IdRol;
                User_Update.ItsActive = UserParameter.ItsActive;
                bool ResultToOperation = await this._GenericPrincipalRepository.UpdateSpecificInformation(User_Update);

                if (!ResultToOperation)
                {
                    throw new TaskCanceledException("No se pudo 'Modificar' el Usuario!!");
                }

                return GetUserInformationInDatabase.Include(RolUserInclude => RolUserInclude.IdRolNavigation).First();
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> RemoveUserInApplication(int IdUserParameter)
        {
            try
            {
                User ConsultUserSpecific = await this._GenericPrincipalRepository.GetSpecificInformation(SearchInfo => SearchInfo.IdUser == IdUserParameter);
                if(ConsultUserSpecific == null) 
                { 
                  throw new TaskCanceledException("El Usuario no fue encontrado");
                }

                string NamePhotoUserInFirebase = ConsultUserSpecific.NamePhoto;
                bool ResultToOperation = await this._GenericPrincipalRepository.DeleteSpecificInformation(ConsultUserSpecific);
                if(!ResultToOperation)
                {
                    throw new TaskCanceledException("El Usuario no pudo ser 'ELIMINADO'...");
                }
                await this._FirebaseStorageRepository.RemoveStorage("carpeta_usuario", NamePhotoUserInFirebase);
                return true;
            }
            catch
            {

                throw;
            }
        }

        public async Task<User> GetbyUserCredentials(string EmailParameter, string PasswordParameter)
        {
            string EncryptPasswordForUser = this._ServicesProfitsRepository.PasswordEncryption(PasswordParameter);
            var GetInformationFoUser = await this._GenericPrincipalRepository.GetSpecificInformation(SearchInfo => SearchInfo.Email.Equals(EmailParameter)
            && SearchInfo.UserPassword.Equals(EncryptPasswordForUser));
            return GetInformationFoUser;
        }

        public async Task<User> GetUserbyId(int IdUser)
        {
            IQueryable<User> ConsultUserInformationById = await this._GenericPrincipalRepository.ConsultSpecificInformation(SearchInfo => 
            SearchInfo.IdUser == IdUser);
            return ConsultUserInformationById.Include(IncludeRolForUser => IncludeRolForUser.IdRolNavigation).FirstOrDefault();
        }
        public async Task<bool> SaveUserProfile(User user)
        {
            try
            {
                var GetUserExistsById = await this._GenericPrincipalRepository.GetSpecificInformation(SearchInfo => SearchInfo.IdUser == user.IdUser);
                if(GetUserExistsById == null) 
                {
                    throw new TaskCanceledException("El Usuario no Existe");
                }

                GetUserExistsById.Email = user.Email;
                GetUserExistsById.Phone = user.Phone;

                bool ResultToOperation = await this._GenericPrincipalRepository.UpdateSpecificInformation(GetUserExistsById);
                return ResultToOperation;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> ChangeUserPassword(int IdUserParameter, string CurrentUserPassword, string NewUserPassword)
        {
            try
            {
                User GetUserExistsById = await this._GenericPrincipalRepository.GetSpecificInformation(SearchInfo => SearchInfo.IdUser == IdUserParameter);
                if (GetUserExistsById == null)
                {
                    throw new TaskCanceledException("El Usuario no Existe");
                }
                if(GetUserExistsById.UserPassword != this._ServicesProfitsRepository.PasswordEncryption(CurrentUserPassword))
                {
                    throw new TaskCanceledException("La Password Ingresada no es 'CORRECTA'");
                }

                GetUserExistsById.UserPassword = this._ServicesProfitsRepository.PasswordEncryption(NewUserPassword);
                bool ResultToOperation = await this._GenericPrincipalRepository.UpdateSpecificInformation(GetUserExistsById);
                return ResultToOperation;

                
            }
            catch(Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> ResetUserPassword(string UserEmail, string UrlMailTemplate)
        {
            try
            {
                User GetUserExistsByMail = await this._GenericPrincipalRepository.GetSpecificInformation(SearchInfo => SearchInfo.Email == UserEmail);
                if(GetUserExistsByMail == null)
                {
                    throw new TaskCanceledException("No Encontramos ningun usuario asociado al correo ingresado..");
                }

                string TemporalityPasswordForUser = this._ServicesProfitsRepository.GeneratePassword();
                GetUserExistsByMail.UserPassword = this._ServicesProfitsRepository.PasswordEncryption(TemporalityPasswordForUser);
              
                    UrlMailTemplate = UrlMailTemplate.Replace("[password]", TemporalityPasswordForUser);
                    string htmlMail = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlMailTemplate);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader streamReader = null;
                            if (response.CharacterSet == null)
                            {
                                streamReader = new StreamReader(dataStream);
                            }
                            else
                            {
                                streamReader = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            htmlMail = streamReader.ReadToEnd();
                            response.Close();
                            streamReader.Close();
                        }
                    }
                bool SendMail = false;
                if (htmlMail != string.Empty || htmlMail != null)
                {
                    SendMail = await this._ServicesMailRepository.SendMailApplication(UserEmail, "Password Restablecida", htmlMail);
                }
                if(SendMail == false)
                {
                    throw new TaskCanceledException("Ocurrio un Problema al Enviar el Restablecimineto de la Password, Porfavor Intentalo mas Tarde..");
                }

                bool ResultToOperation = await this._GenericPrincipalRepository.UpdateSpecificInformation(GetUserExistsByMail);
                return ResultToOperation;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
