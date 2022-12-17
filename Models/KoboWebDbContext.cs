using Microsoft.EntityFrameworkCore;

namespace kobowebmvp_backend_dotnet.Models
{
    public class KoboWebDbContext : DbContext
    {
        public KoboWebDbContext(DbContextOptions<KoboWebDbContext> options) 
        : base(options)
        { 
        }

        public DbSet<FieldAgent> FieldAgents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }

    }
}
