


using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PVA_YearOfRegAPIController : ControllerBase
    {

        private readonly IPVAYearOfRegService _yearOfRegService;

        public PVA_YearOfRegAPIController( IPVAYearOfRegService yearOfRegService)
        {
            
            _yearOfRegService = yearOfRegService;
        }
        /// <summary>
        /// this controller method is used to get year of registration of car from database
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetYearOfRegDetails()
        {
            try
            {
                var yearOfRegDetailsDto = await _yearOfRegService.GetYearOfRegDetailsAsync();
                if(yearOfRegDetailsDto == null)
                {
                    return NotFound("Do not found any year of registeration");
                }
                return Ok(yearOfRegDetailsDto);
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