using Microsoft.EntityFrameworkCore;

namespace server.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppData> AppDatas {get; set;}
    }
}