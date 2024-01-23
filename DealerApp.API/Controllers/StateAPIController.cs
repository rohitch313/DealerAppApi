using AutoMapper;



using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StateAPIController : ControllerBase
    {


        private readonly IStateService _stateService;

        public StateAPIController( IStateService stateService)
        {

            _stateService = stateService;
        }
        /// <summary>
        /// This method is used to get list of state from  service and show in dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStateDetails()
        {
            try
            {
                var statesDto = await _stateService.GetStateDetailsAsync();
                if(statesDto == null)
                {
                    return NotFound("Do not find any state");
                }
                return Ok(statesDto);
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
