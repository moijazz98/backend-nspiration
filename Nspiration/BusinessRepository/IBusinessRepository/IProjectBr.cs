using Nspiration.Response;

namespace Nspiration.BusinessRepository.IBusinessRepository
{
    public interface IProjectBr
    {
        public Task<ProjectInfoResponseModel?> GetProjectInfo(int requstId);
    }
}
