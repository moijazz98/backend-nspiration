using System.ComponentModel.DataAnnotations;

namespace Nspiration.Response
{
    public class ProjectListResponse
    {
        public int oldProjectId { get; set; }
        public string sProjectName { get; set; }
        public string sSiteImage { get; set; }
        public DateTime dtActionDate { get; set; }
        public long ProjectId { get; set; }
    }
}
