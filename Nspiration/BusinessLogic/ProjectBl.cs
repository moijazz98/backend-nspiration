using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic
{
    public class ProjectBl:IProjectBl
    {
        private readonly IProjectBr projectBr;
        public ProjectBl(IProjectBr _projectBr)
        {
            projectBr = _projectBr;
        }

       

        public async Task<ProjectInfoResponseModel?> GetProjectInfo(int requstId)
        {
            return await projectBr.GetProjectInfo(requstId);
        }

        public async Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest)
        {
            return await projectBr.GetVendorProjectList(projRequest);             
        }
        public async Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequestModel fromGimpRequest)
        {
            return await projectBr.AddProjectDetailsFromGimp(fromGimpRequest);
        }
    }
}
