namespace Nspiration.Model
{
    public class ImageInstance
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public int TypeId { get; set; }
        public string? SVG_String { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
