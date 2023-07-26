using Microsoft.EntityFrameworkCore;

namespace Nspiration.Model
{
    [Keyless]
    public class tblProjectTx
    {
        public int iProjectId { get; set; }
        public string sProjectName { get; set; }
        public string? sEmail { get; set; }
        public string? sMobile { get; set; }
        public string? sBuildingName { get; set; }
        public int iAssignedToColorTeam { get; set; }
        public int iCreatedBy { get; set; }
        //public string? SalesOfficerPhoneNumber { get; set; }
        //public string DealerPhoneNumber { get; set; }
    }
}
