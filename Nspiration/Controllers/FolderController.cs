using Microsoft.AspNetCore.Mvc;
using Nspiration.BusinessLogic;
using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.Request;

namespace Nspiration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : Controller
    {
        private readonly IFolderBl folderBl;
        public FolderController(IFolderBl _folderBl)
        {
            folderBl = _folderBl;
        }
        [HttpPost("addfolder")]
        public async Task<IActionResult> AddFolder([FromBody] FolderRequest folderRequestModel)
        {
            var response = await folderBl.AddFolder(folderRequestModel);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
        [HttpGet("getallfolder")]
        public async Task<IActionResult> GetAllFolder(long projectId)
        {
            var response = await folderBl.GetAllFolder(projectId);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
        [HttpPut("deletefolder")]
        public async Task<IActionResult> DeleteFolder([FromBody] DeleteFolderRequest deleteFolder)
        {
            var response=await folderBl.DeleteFolder(deleteFolder);
            if(response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
        [HttpPut("renamefolder")]
        public async Task<IActionResult> RenameFolder([FromBody] RenameFolderRequest renameFolder)
        {
            var response = await folderBl.RenameFolder(renameFolder);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
        //[HttpGet("fetfolderwithsection")]
        //public async Task<IActionResult> GetFolderWithSection(long projectId)
        //{
        //    var response = await folderBl.GetFolderWithSection(projectId);
        //    if (response != null)
        //    {
        //        return Ok(response);
        //    }
        //    return NoContent();
        //}
        [HttpGet("getfolderwithsection")]
        public async Task<IActionResult> GetFolderWithSection(long projectId)
        {
            var response = await folderBl.GetFolderWithSection(projectId);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
    }
}
