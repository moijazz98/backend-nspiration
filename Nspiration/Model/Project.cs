using System.ComponentModel.DataAnnotations.Schema;

namespace Nspiration.Model
{
    public class Project
    {
        public long Id { get; set; }
        public string? Base64_String { get; set; }
        public string? SVG_String { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long ExistingProjectId { get; set; }
        [ForeignKey("ExistingProjectId")]
        public ExistingProject? ExistingProject{ get; set; }
        public ICollection<ImageInstance> ImageInstance { get; set; }
        public ICollection<Section> Section { get; set; }

    }
}
