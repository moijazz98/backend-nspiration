using System.ComponentModel.DataAnnotations;

namespace Nspiration.Response
{
    public class ProjectListResponse
    {
        public int iProjectId { get; set; }
        public string sProjectName { get; set; }
        public string sSiteImage { get; set; }
        public DateTime dtActionDate { get; set; }
    }
}
