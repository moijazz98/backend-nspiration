namespace Nspiration.Request
{
    public class PythonFlaskApiRequest
    {
        public long ProjectId { get; set; }
        public int TypeId { get; set; }
        public List<long> SectionId { get; set; }
        public int ColorId { get; set; }
        public int VendorId { get; set; }
        //public string? PathName { get; set; }
        //public string? ColorCodeId { get; set; }
    }
}
