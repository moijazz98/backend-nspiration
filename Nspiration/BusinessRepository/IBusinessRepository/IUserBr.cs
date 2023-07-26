using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessRepository.IBusinessRepository
{
    public interface IUserBr
    {
        public Task<UserLoginResponse> LoginValidation(UserLoginRequest userLoginRequest);
    }
}
