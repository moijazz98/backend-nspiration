using System.ComponentModel.DataAnnotations.Schema;

namespace Nspiration.Model
{
    public class ProjectExistingToNew
    {
        public long Id { get; set; } 
        public int ProjectRequestId { get; set; }
        public int VendorId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set;}
        public Project Project { get; set; }
    }
}
