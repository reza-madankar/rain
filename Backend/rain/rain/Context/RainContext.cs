using Microsoft.EntityFrameworkCore;
using Rain.Model;
using Rain.Context.Model;
using System.Reflection.Emit;

namespace Rain.Context
{
    public class RainContext : DbContext
    {
        public RainContext(DbContextOptions<RainContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RainEntity>().HasIndex(r => r.UserId);
        }

        public DbSet<RainEntity> RainEntities { get; set; }
    }
}
