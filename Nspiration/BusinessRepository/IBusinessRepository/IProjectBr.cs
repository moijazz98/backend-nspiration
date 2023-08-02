using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessRepository.IBusinessRepository
{
    public interface IProjectBr
    {
        public Task<ProjectInfoResponseModel?> GetProjectInfo(int requstId);
        public Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest);
        public Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequestModel fromGimpRequest);
    }
}
