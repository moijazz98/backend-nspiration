using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic.IBusinessLogic
{
    public interface IUserBl
    {
        public Task<UserLoginResponse> LoginValidation(UserLoginRequest userLoginRequest);
    }
}
