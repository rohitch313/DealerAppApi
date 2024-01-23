using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class UploadPicController : Controller
    {
        private readonly IUploadPicServices _uploadPicServices;

        public UploadPicController(IUploadPicServices uploadPicServices)
        {
            _uploadPicServices = uploadPicServices;
        }

        [HttpPost("uploadingPic")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public

    async Task<IActionResult> UploadPic(UploadPic_StockAuditDTO request)
        {
            try
            {
                var response = await _uploadPicServices.UploadPicAsync(request);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response.Message);// adjust status code depending on error specificty
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
