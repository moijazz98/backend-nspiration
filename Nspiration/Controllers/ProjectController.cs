using Microsoft.AspNetCore.Mvc;
using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.Request;
using System;

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
        public async Task<IActionResult> GetProjectInfo(int projectRequestId)
        {
            var response = await projectBl.GetProjectInfo(projectRequestId);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }

        [HttpPost("GetProjectList")]
        public async Task<IActionResult> VendorProjectList([FromBody] ProjectListRequest projRequest)
        {
            if (projRequest.VendorId != null && projRequest.VendorId !=0)
            {
                var getuserList = await projectBl.GetVendorProjectList(projRequest);
                return Ok(getuserList);
            }
            else
            {
                return NotFound("Invaid VendorId!!!!");
            }                         
        }
        [HttpPost("addprojectdetails")]
        public async Task<IActionResult> AddProjectDetails([FromBody] FromGimpRequest fromGimpRequest)
        {
            var response = await projectBl.AddProjectDetailsFromGimp(fromGimpRequest);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }

        [HttpPost("GetprojectSection")]
        public async Task<IActionResult> GetprojectSection([FromBody] ProjectListRequest pRequest)
        {
            var response = await projectBl.GetprojectSection(pRequest);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }

        [HttpPost("GetprojectRep")]
        public async Task<IActionResult> GetprojectRep(long projectId, int typeId)
        {
            var response = await projectBl.GetprojectRep(projectId, typeId);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }

        [HttpPost("GetPdfData")]
        public async Task<IActionResult> GetPdfData(long projectId, int typeId)
        {
            var response = await projectBl.GetPdfData(projectId, typeId);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
        [HttpPost("CallPythonFlaskApi")]
        public async Task<IActionResult> CallPythonFlaskApi([FromBody] PythonFlaskApiRequest pythonFlaskApiRequest)
        {
            var response = await projectBl.CallPythonFlaskApi(pythonFlaskApiRequest);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
    }
}
