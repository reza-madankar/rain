using Microsoft.EntityFrameworkCore;
using rain.Context.Model;
using rain.Context.Services;

namespace rain.Context.Repository
{
    public class RainRepository: IRain
    {
        Task<IEnumerable<RainEntity>> getRains(bool? isRain = null, string userId = "", int page = 1)
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

            var projectedQuery = query.Select(r => new RainEntity
            {
                Timestamp = r.Timestamp,
                Rain = r.Rain,
                UserId = r.UserId
            });

            var result = await projectedQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }

        Task<bool> AddRain(bool isRainy)
        {

        }
    }
}
