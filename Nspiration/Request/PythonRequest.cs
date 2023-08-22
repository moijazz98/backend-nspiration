namespace Nspiration.Request
{
    public class PythonRequest
    {
        public string? SVG_String { get; set; }
        public string? Base64_String { get; set; }
        public long ProjectId { get; set; }
        //public string? ColorCodeId { get; set; }
        //public string? PathName { get; set; }
        //public int TypeId { get; set; }
        public List<SectionAndColorResponse> SectionAndColorResponse { get; set;}
    }
}
