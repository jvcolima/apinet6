using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JwtWebApi.Services.UserService
{
    // UserService.cs
    public class UserService : IUserService
    {
        private readonly UserDbContext _dbContext; // Reemplaza "YourDbContext" con el nombre de tu contexto de base de datos.

        public UserService(UserDbContext dbContext) // Inyecta tu contexto de base de datos.
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }


        public List<User> GetUsers()
        {
            // Realiza una consulta a la base de datos para obtener todos los usuarios
            List<User> users = _dbContext.Users.ToList();

            return users;
        }


    }
}