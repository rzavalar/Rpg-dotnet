using dotnet__rpg.Services.CharacterService;

namespace dotnet__rpg.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);

        Task<ServiceResponse<string>> Login(string username, string password);

        Task<bool> UserExist(string username);
         
    }
}