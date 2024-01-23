
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DealerApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentProofImgController : ControllerBase
    {
        private readonly ILogger<PaymentProofImgController> _logger;
        private readonly IPaymentProofImgService _paymentProofImgService;

        public PaymentProofImgController(ILogger<PaymentProofImgController> logger, IPaymentProofImgService paymentProofImgService)
        {
            _logger = logger;
            _paymentProofImgService = paymentProofImgService;
        }

        [HttpPost("PaymentPic/{id}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadPic(int id, [FromBody] PaymentProofImgDTO proofImgDTO)
        {
            try
            {
                var result = await _paymentProofImgService.UploadPaymentProofImageAsync(id, proofImgDTO);

                if (result == "Upload successful")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing the request");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}