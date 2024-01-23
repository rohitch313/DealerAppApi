using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseVehicleController : Controller
    {
        private readonly IPurchaseVehicleService _carService;

        public PurchaseVehicleController(IPurchaseVehicleService carService)
        {
            _carService = carService;
        }

        [HttpGet("VehicleRecord/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public

    async Task<IActionResult> GetVehicleRecordStatus(int id)
        {
            try
            {
                var response = await _carService.GetVehicleRecordStatusAsync(id);

                if (response.Success)
                {
                    return Ok(response.Data);
                }
                else if (response.Message == "Vehicle record not found")
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, response.Message);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
        }
        [HttpGet("VehicleRecord")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCarsWithStatus()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _carService.GetCarsWithStatusAsync(userIdString);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
    }
}

