using JwtWebApi;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    // Otros DbSets y configuraciones específicas de tu aplicación.
}
