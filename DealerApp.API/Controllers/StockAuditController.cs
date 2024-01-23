using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockAuditController : Controller
    {
        private readonly IStockAuditService _stockAuditService;

        public StockAuditController(IStockAuditService stockAuditService)
        {
            _stockAuditService = stockAuditService;
        }

        [HttpGet("upcoming")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public

    async Task<IActionResult> GetUpcomingAudits()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _stockAuditService.GetUpcomingAuditsAsync(userIdString);
                if (response.Success)
                {
                    return Ok(response.Data);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("pending")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public

    async Task<IActionResult> GetPendingAudits()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _stockAuditService.GetPendingAuditsAsync(userIdString);
                if (response.Success)
                {
                    return Ok(response.Data);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("StockStatus")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockStatus()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _stockAuditService.GetStockStatusAsync(userIdString);
                if (response.Success)
                {
                    return Ok(response.Data);
                }else { return BadRequest(response); }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error:{ex.Message} ");
            }
        }
        [HttpGet("addresses")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public

async Task<IActionResult> GetUserAddresses()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _stockAuditService.GetUserAddressesAsync(userIdString);
                return Ok(response.Data);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
    }
}

