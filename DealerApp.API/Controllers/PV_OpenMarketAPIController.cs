using AutoMapper;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DealerApp.API.Controllers
{
    //public class PV_OpenMarketAPIController
    //{
    //}

    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class PV_OpenMarketAPIController : ControllerBase
    {
        private readonly ILogger<PV_OpenMarketAPIController> _logger;
        private readonly IPVOpenMarketService _openMarketService;

        public PV_OpenMarketAPIController(ILogger<PV_OpenMarketAPIController> logger, IPVOpenMarketService openMarketService)
        {
            _logger = logger;
            _openMarketService = openMarketService;
        }

        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PV_OpenMarketDTO>>> GetOpenMarketSupport()
        {
            try
            {
                var openMarketDto = await _openMarketService.GetOpenMarketSupportAsync();
                return Ok(openMarketDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Return an appropriate error response
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// this controller method is used to post  purchase vehicle through service
        /// </summary>
        /// <param name="pV_openmarketDTO"></param>
        /// <returns></returns>

        [HttpPost("Post")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PV_OpenMarketDTO>> PostOpenMarketSupport(PV_OpenMarketDTO pV_openmarketDTO)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdString, out int userId))
                {
                    var result = await _openMarketService.PostOpenMarketSupportAsync(pV_openmarketDTO, userId);
                    return Ok(result);
                }
                else
                {
                    // Handle the case where the user ID from the claim cannot be parsed as an integer
                    return BadRequest("Invalid user ID");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Return an appropriate error response
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
