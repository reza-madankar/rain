using System.Reflection.Emit;

namespace rain.Context
{
    public class RainContext : DbContext
    {
        public RainContext(DbContextOptions<LavenderEventsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
