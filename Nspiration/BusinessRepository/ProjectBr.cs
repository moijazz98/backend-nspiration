using Microsoft.EntityFrameworkCore;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.NspirationDBContext;
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
    }
}
