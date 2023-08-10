namespace Nspiration.Response
{
    public class PdfBodyResponse
    {
        public string SVG_String { get; set; }
        public string Base64_image { get; set; }
        public List<PdfProjectReponse> projectData { get; set; }

    }   
}
