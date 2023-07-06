using Nspiration.BusinessLogic.IBusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Nspiration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        
        private readonly IColorBl colorBl;
        public ColorController(IColorBl _icolorBl)
        {
            colorBl = _icolorBl;
        }
        [HttpGet("getallcolors")]
        public async Task<IActionResult> GetAllColors()
        {
            var response=await colorBl.GetAllColors();
            if(response!=null)
            {
                return Ok(response);
            }
            return NoContent();
        }
        [HttpGet("getcolorsbyfamily")]
        public async Task<IActionResult> GetColorsByFamily(int familyId)
        {
            var response = await colorBl.GetColorsByFamily(familyId);
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }

        [HttpGet("getallfamilies")]
        public async Task<IActionResult> GetFamilyList()
        {
            var response = await colorBl.GetFamilyList();
            if (response != null)
            {
                return Ok(response);
            }
            return NoContent();
        }
    }
}
