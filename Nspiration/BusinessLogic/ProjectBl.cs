using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
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
    }
}
