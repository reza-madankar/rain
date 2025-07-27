using rain.Context.Model;
using rain.Model;

namespace rain.Context.Services
{
    public interface IRain
    {
        Task<RainListDto> GetRains(bool? isRain = null, string userId = "", int page = 1, int pageSize = 10);

        Task<bool> AddRain(bool isRainy, string userId);
    }
}
