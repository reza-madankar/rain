using Microsoft.EntityFrameworkCore;
using rain.Context.Model;
using rain.Context.Services;
using rain.Model;

namespace rain.Context.Repository
{
    public class RainRepository : IRain
    {
        private readonly RainContext _context;
        public RainRepository(RainContext context)
        {
            _context = context;
        }

        public async Task<RainListDto> GetRains(bool? isRain = null, string userId = "", int page = 1, int pageSize = 10)
        {
            IQueryable<RainEntity> query = _context.RainEntities;

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(r => r.UserId == userId);
            }

            if (isRain.HasValue)
            {
                query = query.Where(r => r.Rain == isRain.Value);
            }

            var totalRecords = await query.CountAsync();

            var projectedQuery = query.Select(r => new RainEntity
            {
                Timestamp = r.Timestamp,
                Rain = r.Rain,
                UserId = r.UserId
            });

            var result = await projectedQuery
                .OrderByDescending(r => r.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return new RainListDto
            {
                Rain = result,
                CurrentPage = page,
                TotalRecords = totalRecords
            };

        }

        public async Task<bool> AddRain(bool isRainy, string userId)
        {
            try
            {
                var rainEntry = new RainEntity
                {
                    Rain = isRainy,
                    UserId = userId,
                    Timestamp = DateTime.UtcNow
                };

                _context.RainEntities.Add(rainEntry);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
