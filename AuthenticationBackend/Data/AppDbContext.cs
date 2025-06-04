using JWTAuth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        
    }
}
