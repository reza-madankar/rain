using Microsoft.EntityFrameworkCore;
using rain.Context.Model;
using rain.Model;

namespace rain.Context
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
