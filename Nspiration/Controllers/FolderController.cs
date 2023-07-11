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
        public FolderController(IFolderBl _ifolderBl)
        {
            folderBl = _ifolderBl;
        }
        [HttpPost("addfolder")]
        public async Task<IActionResult> AddFolder([FromBody] FolderRequestModel folderRequestModel)
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
        public async Task<IActionResult> DeleteFolder([FromBody] DeleteFolderRequestModel deleteFolder)
        {
            var response=await folderBl.DeleteFolder(deleteFolder);
            if(response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
        [HttpPut("renamefolder")]
        public async Task<IActionResult> RenameFolder([FromBody] RenameFolderRequestModel renameFolder)
        {
            var response = await folderBl.RenameFolder(renameFolder);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
    }
}
