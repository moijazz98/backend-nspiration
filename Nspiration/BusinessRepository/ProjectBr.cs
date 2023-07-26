using Microsoft.EntityFrameworkCore;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.NspirationDBContext;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessRepository
{
    public class ProjectBr:IProjectBr
    {
        public readonly NspirationPortalOldDBContext nspirationPortalOldDBContext;
        public ProjectBr(NspirationPortalOldDBContext _nspirationPortalOldDBContext)
        {
            nspirationPortalOldDBContext = _nspirationPortalOldDBContext;
        }

        public async Task<ProjectInfoResponseModel?> GetProjectInfo(int requestId)
        {
            ProjectInfoResponseModel? projectInfo = await (from project in nspirationPortalOldDBContext.tblProjectTx
                                                           join user in nspirationPortalOldDBContext.tblUserM
                                                           on project.iCreatedBy equals user.iUserId
                                                           where project.iProjectId == requestId
                                                           select new ProjectInfoResponseModel
                                                           {
                                                               Id = project.iProjectId,
                                                               ProjectName = project.sProjectName,
                                                               Email = project.sEmail,
                                                               CustomerPhoneNumber = project.sMobile,
                                                               Address = project.sBuildingName,
                                                               SalesOfficerPhoneNumber = user.sPhone,
                                                               DealerPhoneNumber = "12345",
                                                           }).FirstOrDefaultAsync();
            return projectInfo;
        }

        public async Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest)
        {
            try
            {
                string startupPath = System.IO.Directory.GetCurrentDirectory();

                List<ProjectListResponse> projectList = await(from p in nspirationPortalOldDBContext.tblProjectTx
                                                              join d in nspirationPortalOldDBContext.tblDepotM
                                                              on p.DepotId equals d.DepotId
                                                              where d.OperationalTeamId == projRequest.VendorId &&
                                                              (projRequest.ProjectId == 0 || p.iProjectId == projRequest.ProjectId)
                                                              orderby p.dtCreationDate descending
                                                              select new ProjectListResponse
                                                              {
                                                                  iProjectId = p.iProjectId,
                                                                  sProjectName = p.sProjectName,
                                                                  sSiteImage = (p.sSiteImage == null ? "Image not found" : startupPath + @"\\ProjectImages\\Site-sampleImg.jpg"),
                                                                  dtActionDate = p.dtActionDate

                                                              }).Take(10).ToListAsync();

                return projectList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
