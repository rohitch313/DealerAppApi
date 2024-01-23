using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;


namespace DealerApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet("due")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDuePayments()
        {
            try
            {
                _logger.LogInformation("Getting Due Payments");

                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdString, out int userId))
                {
                    var duePayments = await _paymentService.GetDuePayments(userId);
                    return Ok(duePayments);
                }
                else
                {
                    // Handle the case where the user ID from the claim cannot be parsed as an integer
                    return BadRequest("Invalid user ID");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting due payments: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("upcoming")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUpcomingPayments()
        {
            try
            {
                _logger.LogInformation("Getting Upcoming Payments");

                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdString, out int userId))
                {
                    var upcomingPayments = await _paymentService.GetUpcomingPayments(userId);
                    return Ok(upcomingPayments);
                }
                else
                {
                    // Handle the case where the user ID from the claim cannot be parsed as an integer
                    return BadRequest("Invalid user ID");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting upcoming payments: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentStatus()
        {
            try
            {
                _logger.LogInformation("Getting Payment Status");

                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdString, out int userId))
                {
                    var paymentStatus = await _paymentService.GetPaymentStatus(userId);
                    return Ok(paymentStatus);
                }
                else
                {
                    // Handle the case where the user ID from the claim cannot be parsed as an integer
                    return BadRequest("Invalid user ID");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting payment status: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("details/{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentDetailsWithBankDetails(int paymentId)
        {
            try
            {
                _logger.LogInformation($"Getting Payment Details with Bank Details for Payment ID: {paymentId}");

                var paymentDetails = await _paymentService.GetPaymentDetailsWithBankDetails(paymentId);

                if (paymentDetails == null)
                {
                    return NotFound();
                }

                return Ok(paymentDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting payment details: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}