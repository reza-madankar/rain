using rain.Context.Model;

namespace rain.Context.Services
{
    public interface IRain
    {
        Task<IEnumerable<RainEntity>> getRains(bool? isRain = null, string userId = "", int page = 1, int pageSize = 10);

        Task<bool> AddRain(bool isRainy);
    }
}
