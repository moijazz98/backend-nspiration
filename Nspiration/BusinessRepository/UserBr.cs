using Microsoft.IdentityModel.Tokens;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Helpers;
using Nspiration.NspirationDBContext;
using Nspiration.Request;
using Nspiration.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nspiration.BusinessRepository
{
    public class UserBr:IUserBr
    {
        private IConfiguration config;
        public readonly NspirationPortalOldDBContext nspirationPortalOldDBContext;
        public UserBr(IConfiguration _config,NspirationPortalOldDBContext _nspirationPortalOldDBContext)
        {
            nspirationPortalOldDBContext = _nspirationPortalOldDBContext;
            this.config = _config;
        }

        public async Task<UserLoginResponse> LoginValidation(UserLoginRequest userLoginRequest)
        {
            UserLoginResponse userLoginResponse = new UserLoginResponse();

            byte[] b = new byte[1];
            string encryptPassword = SimpleHash.ComputeHash(userLoginRequest.Password, "SHA1", b);
            var user = nspirationPortalOldDBContext.tblUserM.FirstOrDefault(x => x.sPhone == userLoginRequest.PhoneNumber && x.sPassword == encryptPassword && x.sUserType=="6" && x.cStatus=='A');
            if(user !=  null && user.sPassword==userLoginRequest.Password)
            {
                var loginToken = GetJWTToken(userLoginRequest);
                userLoginResponse.GeneratedToken = loginToken;
                userLoginResponse.SuccessorErrorMessage= "Login Successful";
                return userLoginResponse;
            }
            else
            {
                userLoginResponse.SuccessorErrorMessage = "-- Incorrect Credentials --";
                return userLoginResponse;
            }
        }

        private string GetJWTToken(UserLoginRequest userReq)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userReq.PhoneNumber),
                //new Claim("fullName", user.UserName + " " + userReq.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, this.config["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, this.config["Jwt:Issuer"])
            };

            var token = new JwtSecurityToken(
                issuer: this.config["Issuer"],
                audience: this.config["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(300),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
