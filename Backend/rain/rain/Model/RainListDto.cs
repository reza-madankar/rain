using Rain.Context.Model;

namespace Rain.Model
{
    public class RainListDto : Message
    {
        public IEnumerable<RainExportDto> Rain { get; set; } = new List<RainExportDto>();

        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
    }
}
