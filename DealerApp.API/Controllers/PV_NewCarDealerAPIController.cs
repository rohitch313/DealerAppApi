
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DealerApp.API.Controllers

{


    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class PV_NewCarDealerAPIController : ControllerBase
    {
        private readonly ILogger<PV_NewCarDealerAPIController> _logger;
        private readonly IPVNewCarDealerService _newCarDealerService;

        public PV_NewCarDealerAPIController(ILogger<PV_NewCarDealerAPIController> logger, IPVNewCarDealerService newCarDealerService)
        {
            _logger = logger;
            _newCarDealerService = newCarDealerService;
        }

        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PV_NewCarDealerDTO>>> GetNewCarDealerSupport()
        {
            try
            {
                var carDealerDto = await _newCarDealerService.GetNewCarDealerSupportAsync();
                return Ok(carDealerDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Return an appropriate error response
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// this controller method is used to post  purchase vehicle through service in NewCar
        /// </summary>

        [HttpPost("Post")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PV_NewCarDealerDTO>> PostNewCarDealer(PV_NewCarDealerDTO pv_cardealerDTO)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdString, out int userId))
                {
                    var result = await _newCarDealerService.PostNewCarDealerAsync(pv_cardealerDTO, userId);
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