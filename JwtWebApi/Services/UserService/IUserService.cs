using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.Services.UserService
{
    // IUserService.cs
    public interface IUserService
    {

        Task<bool> UserExists(string username);
        Task AddUser(User user);
        
        Task<User> GetUserByUsername(string username);
        List<User> GetUsers();

    }

   
    
}
