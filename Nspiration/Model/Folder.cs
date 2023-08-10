using System.Numerics;

namespace Nspiration.Model
{
    public class Folder
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
