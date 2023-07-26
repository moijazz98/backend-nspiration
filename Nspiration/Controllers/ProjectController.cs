using Microsoft.AspNetCore.Mvc;
using Nspiration.BusinessLogic.IBusinessLogic;

namespace Nspiration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IProjectBl projectBl;
        public ProjectController(IProjectBl _projectBl)
        {
            projectBl = _projectBl;
        }
        [HttpGet("getprojectinfo")]
        public async Task<IActionResult> GetProjectInfo(int requestId)
        {
            var response = await projectBl.GetProjectInfo(requestId);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
    }
}
