namespace Nspiration.Request
{
    public class FromGimpRequest
    {
        public int ProjectRequestId { get; set; }
        public int VendorId { get; set; }
        public string? Base64_String { get; set; }
        public string? SVG_String { get; set; }
    }
}
