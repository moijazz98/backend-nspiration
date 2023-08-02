namespace Nspiration.Model
{
    public class History
    {
        public long Id { get; set; }
        public long ImageInstanceId { get; set; }
        public string? SVG_string { get; set; }
        public long ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
