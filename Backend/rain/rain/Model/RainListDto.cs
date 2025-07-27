using rain.Context.Model;

namespace rain.Model
{
    public class RainListDto : Message
    {
        public IEnumerable<RainEntity> Rain { get; set; } = new List<RainEntity>();

        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
    }
}
