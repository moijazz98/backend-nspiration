using Nspiration.Model;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic.IBusinessLogic
{
    public interface IProjectBl 
    {
        public Task<ProjectInfoResponse?> GetProjectInfo(int requstId);
        public Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest);
        public Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequest fromGimpRequest);
        public Task<List<SectionResponse>> GetprojectSection(ProjectListRequest pRequest);
    }
}
