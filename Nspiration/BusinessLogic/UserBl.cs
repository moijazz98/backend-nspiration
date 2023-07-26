using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic
{
    public class UserBl:IUserBl
    {
        private readonly IUserBr userBr;
        public UserBl(IUserBr _userBr)
        {
            userBr = _userBr;
        }

        public async Task<UserLoginResponse> LoginValidation(UserLoginRequest userLoginRequest)
        {
            return await userBr.LoginValidation(userLoginRequest);
        }
    }
}

