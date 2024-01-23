
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class PVA_VariantAPIController : ControllerBase
    {

        private readonly IPVAVariantService _variantService;

        public PVA_VariantAPIController( IPVAVariantService variantService)
        {

            _variantService = variantService;
        }
        /// <summary>
        /// this controller method is used to get variant of car from service
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetVariantDetails()
        {
            try
            {
                var variantDetailsDto = await _variantService.GetVariantDetailsAsync();
                if (variantDetailsDto == null)
                {
                    return NotFound("Do not found any  variant");
                }
                return Ok(variantDetailsDto);
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