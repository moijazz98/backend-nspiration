using Microsoft.AspNetCore.Mvc;
using Nspiration.BusinessLogic;
using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.Request;

namespace Nspiration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public readonly IUserBl userBl;
        public UserController(IUserBl _userBl)
        {
                userBl = _userBl;
        }
        [HttpPost("getLoginValidation")]
        public async Task<IActionResult> LoginValidation([FromBody] UserLoginRequest userLoginRequest)
        {
            var response = await userBl.LoginValidation(userLoginRequest);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
    }
}
