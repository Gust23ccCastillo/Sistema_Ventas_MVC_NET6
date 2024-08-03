using SistemaVentas.Entities;

namespace SistemaVentas.BusinessLogic.Interface
{
    public interface IServicesUsers
    {
        Task<List<User>> GetListUsersInApplication();
        Task<User> CreateUserInApplication(User UserParameter, Stream UserPhotoParameter = null, string NamePhotoParameter = "",string UrlMailTemplate = "");
        Task<User> UpdateUserInApplication(User UserParameter, Stream UserPhotoParameter = null, string NamePhotoParameter = "");
        Task<bool> RemoveUserInApplication(int IdUserParameter);
        Task<User> GetbyUserCredentials(string EmailParameter, string PasswordParameter);
        Task<User> GetUserbyId(int IdUser);
        Task<bool> SaveUserProfile(User user);
        Task<bool> ChangeUserPassword(int IdUserParameter, string CurrentUserPassword, string NewUserPassword);
        Task<bool> ResetUserPassword(string UserEmail,string UrlMailTemplate);
    }
}
