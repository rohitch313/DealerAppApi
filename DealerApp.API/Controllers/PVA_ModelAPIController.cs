

using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PVA_ModelAPIController : ControllerBase
    {

        private readonly IPVAModelService _modelService;

        public PVA_ModelAPIController( IPVAModelService modelService)
        {
        
            _modelService = modelService;
        }

        [HttpGet]

        public async Task<IActionResult> GetModelDetails()
        {
            try
            {
                var modelDetailsDto = await _modelService.GetModelDetailsAsync();
                if (modelDetailsDto == null)
                {
                    return NotFound("Do not found any model of car");
                }
                return Ok(modelDetailsDto);
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


