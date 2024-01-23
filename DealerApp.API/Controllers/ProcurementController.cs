using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DealerApp.API.Controllers
{
      [Route("api/[controller]")]
    [ApiController]
    public class ProcurementController : ControllerBase
    {
        private readonly IProucurementService _procurementService;

        public ProcurementController(IProucurementService procurementService)
        {
            _procurementService = procurementService;
        }

        [HttpGet("filter")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFilter()
        {
            try
            {
                var response = await _procurementService.GetFiltersAsync();
                if (response.Success)
                {
                    return Ok(response.Data); // Or return a more specific success response
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                // ... (additional logging if needed)
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Procurement")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public async Task<IActionResult> FilterProcurement(int? Id)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _procurementService.GetProcurementsAsync(Id, userIdString);
                if (response.Success)
                {
                    return Ok(response.Data); // Or return a more specific success response
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet("ProcurementStatus")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProcurementStatus()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _procurementService.GetProcurementStatusAsync(userIdString);

                if (response.Success)
                {
                    return Ok(response.Data); // Or return a more specific success response
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
        }
        [HttpGet("ProcurementClosed")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public

async Task<IActionResult> ProcurementClosed()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _procurementService.GetProcurementClosedAsync(userIdString);
                if (response.Success)
                {
                    return Ok(response.Data); // Or return a more specific success response
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}

