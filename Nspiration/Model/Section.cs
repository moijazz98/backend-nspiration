namespace Nspiration.Model
{
    public class Section
    {
        public long Id { get; set; }
        public long? FolderId { get; set; }
        public int ProjectId { get; set; }
        public string? PathName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
