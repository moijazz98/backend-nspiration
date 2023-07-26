using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.NspirationDBContext;
using Nspiration.Request;
using Nspiration.Response;
using System.Text;

namespace Nspiration.BusinessRepository
{
    public class UserBr:IUserBr
    {
        public readonly NspirationPortalOldDBContext nspirationPortalOldDBContext;
        public UserBr(NspirationPortalOldDBContext _nspirationPortalOldDBContext)
        {
            nspirationPortalOldDBContext = _nspirationPortalOldDBContext;
        }

        public async Task<UserLoginResponse> LoginValidation(UserLoginRequest userLoginRequest)
        {
            UserLoginResponse userLoginResponse = new UserLoginResponse();
            var user = nspirationPortalOldDBContext.tblUserM.FirstOrDefault(x => x.sPhone == userLoginRequest.PhoneNumber && x.sUserType=="6" && x.cStatus=='A');
            if(user !=  null && user.sPassword==userLoginRequest.Password)
            {
                userLoginResponse.UserId = user.iUserId;
                userLoginResponse.SuccessorErrorMessage= "Login Successful";
                return userLoginResponse;
            }
            else
            {
                userLoginResponse.SuccessorErrorMessage = "-- Incorrect Credentials --";
                return userLoginResponse;
            }
        }
    }
}
