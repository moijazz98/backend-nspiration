using Nspiration.Model;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessRepository.IBusinessRepository
{
    public interface IProjectBr
    {
        public Task<ProjectInfoResponse?> GetProjectInfo(int requstId);
        public Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest);
        public Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequest fromGimpRequest);
        public Task<List<SectionResponse>> GetprojectSection(ProjectListRequest pRequest);
        public Task<List<ProjectRepResponse>> GetprojectRep(long projectId, int typeId);
        public Task<PdfDataResponse> GetPdfData(long projectId, int typeId);
        public Task<PythonRequest> CallPythonFlaskApi (PythonFlaskApiRequest pythonFlaskApiRequest);
    }
}
