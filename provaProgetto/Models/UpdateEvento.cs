namespace provaProgetto.Models
{
    public class UpdateEvento
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public int? maxParticipants { get; set; }

    }
}
