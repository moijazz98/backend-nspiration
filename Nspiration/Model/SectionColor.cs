namespace Nspiration.Model
{
    public class SectionColor
    {
        public long Id { get; set; }
        public long SectionId { get; set; }
        public long TypeId { get; set; }
        public int? ColorId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
