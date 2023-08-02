namespace Nspiration.Request
{
    public class ProjectRequestModel
    {
        public int ProjectRequestId { get; set; }
        public string Image { get; set; }
        public string? SVG_Path { get; set; }
        public int CreatedBy { get; set; }
    }
}
