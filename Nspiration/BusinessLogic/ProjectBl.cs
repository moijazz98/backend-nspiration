using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessRepository;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Model;
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

       

        public async Task<ProjectInfoResponse?> GetProjectInfo(int requstId)
        {
            return await projectBr.GetProjectInfo(requstId);
        }

        public async Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest)
        {
            return await projectBr.GetVendorProjectList(projRequest);             
        }
        public async Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequest fromGimpRequest)
        {
            return await projectBr.AddProjectDetailsFromGimp(fromGimpRequest);
        }

        public async Task<List<SectionResponse>> GetprojectSection(ProjectListRequest pRequest)
        {
            return await projectBr.GetprojectSection(pRequest);
        }

        public async Task<List<ProjectRepResponse>> GetprojectRep(long projectId, int typeId)
        {
            return await projectBr.GetprojectRep(projectId, typeId);
        }

        public async Task<PdfDataResponse> GetPdfData(long projectId, int typeId)
        {
            return await projectBr.GetPdfData(projectId, typeId);
        }

        public async Task<PythonRequest> CallPythonFlaskApi(PythonFlaskApiRequest pythonFlaskApiRequest)
        {
            return await projectBr.CallPythonFlaskApi(pythonFlaskApiRequest);
        }
    }
}
