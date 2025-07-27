using System.ComponentModel.DataAnnotations;

namespace Rain.Context.Model
{
    public class RainEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
        public bool Rain { get; init; }
        public string UserId { get; init; } = string.Empty;
    }
}
