namespace Nspiration.Response
{
    public class ProjectImageResponse
    {
        public long Id { get; set; }
        public int TypeId { get; set; }
        public string? Base64_String { get; set; }
        public string? SVG_String { get; set; }
    }
}
