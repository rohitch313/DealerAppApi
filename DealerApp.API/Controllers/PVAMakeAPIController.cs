

using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PVAMakeAPIController : ControllerBase
    {

        private readonly IPVAMakeService _makeService;

        public PVAMakeAPIController( IPVAMakeService makeService)
        {

            _makeService = makeService;
        }
        /// <summary>
        /// This is used to get the list of car make  and show in the dropdown box
        /// </summary>
    
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMakeDetails()
        {
            try
            {
                var makeDetailsDto = await _makeService.GetMakeDetailsAsync();
                if(makeDetailsDto == null)
                {
                    return NotFound("Do not found any MakeDetails");
                }
                return Ok(makeDetailsDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}