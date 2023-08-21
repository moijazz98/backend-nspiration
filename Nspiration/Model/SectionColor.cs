using System.ComponentModel.DataAnnotations.Schema;

namespace Nspiration.Model
{
    public class SectionColor
    {
        public long Id { get; set; }
        public int TypeId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long SectionId { get; set; }
        [ForeignKey("SectionId")]
        public Section Section { get; set; }
        public int? ColorId { get; set; }
        [ForeignKey("ColorId")]
        public Color? Color { get; set; }
    }
}
