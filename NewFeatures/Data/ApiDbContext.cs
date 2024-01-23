using Microsoft.EntityFrameworkCore;
using NewFeatures.Models;

namespace NewFeatures.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
