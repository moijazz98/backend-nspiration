namespace Nspiration.Request
{
    public class SectionColorRequest
    {
        public long Id { get; set; }
        public long TypeId { get; set; }
        public int? ColorId { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public List<long> SectionIds { get; set; }
    }
}
