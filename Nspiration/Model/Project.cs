namespace Nspiration.Model
{
    public class Project
    {
        public long Id { get; set; }
        public long ExistingToNewId { get; set; }
        public string? Base64_String { get; set; }
        public string? SVG_String { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

    }
}
