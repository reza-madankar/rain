using Microsoft.EntityFrameworkCore;
using Rain.Model;
using Rain.Context.Model;

namespace Rain.Context
{
    public class RainContext : DbContext
    {
        public RainContext(DbContextOptions<RainContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<RainEntity> RainEntities { get; set; }
    }
}
