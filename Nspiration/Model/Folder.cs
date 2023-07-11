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
        public long CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
