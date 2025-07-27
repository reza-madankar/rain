using Microsoft.EntityFrameworkCore;
using Rain.Model;
using Rain.Context;
using Rain.Context.Model;
using Rain.Context.Services;

namespace Rain.Context.Repository
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

            var result = await query
                .OrderByDescending(r => r.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RainExportDto
                {
                    Timestamp = r.Timestamp,
                    Rain = r.Rain
                })
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
