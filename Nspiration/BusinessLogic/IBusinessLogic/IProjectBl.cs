using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic.IBusinessLogic
{
    public interface IProjectBl
    {
        public Task<ProjectInfoResponseModel?> GetProjectInfo(int requstId);
        public Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest);
        public Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequestModel fromGimpRequest);
    }
}
