using AutoMapper;
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
    public class PV_AggregatorAPIController : ControllerBase
    {
        private readonly ILogger<PV_AggregatorAPIController> _logger;
        private readonly IPVAggregatorService _aggregatorService;

        public PV_AggregatorAPIController(ILogger<PV_AggregatorAPIController> logger, IPVAggregatorService aggregatorService)
        {
            _logger = logger;
            _aggregatorService = aggregatorService;
        }

        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PV_AggregatorDTO>>> GetAggregatorSupport()
        {
            try
            {
                var aggreatorDto = await _aggregatorService.GetAggregatorSupportAsync();
                return Ok(aggreatorDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Return an appropriate error response
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// this controller method is used to post  purchase vehicle through service in aggregator
        /// </summary>
        [Authorize]
        [HttpPost("Post")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PV_AggregatorDTO>> PostAggregatorSupport(PV_AggregatorDTO pv_aggregatorDTO)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdString, out int userId))
                {
                    var result = await _aggregatorService.PostAggregatorSupportAsync(pv_aggregatorDTO, userId);
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
