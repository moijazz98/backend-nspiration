using Nspiration.Response;

namespace Nspiration.BusinessLogic.IBusinessLogic
{
    public interface IProjectBl
    {
        public Task<ProjectInfoResponseModel?> GetProjectInfo(int requstId);
    }
}
