using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockPurposeController : Controller
    {
        private readonly IStockPurposeService _stockPurposeService;

        public StockPurposeController(IStockPurposeService stockPurposeService)
        {
            _stockPurposeService = stockPurposeService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public

    async Task<IActionResult> GetStockPurposeDetails()
        {
            try
            {
                var stockAPDto = await _stockPurposeService.GetStockPurposeDetailsAsync();
                if(stockAPDto.Success)
                {
                    return Ok(stockAPDto.Data);
                }else
                {
                    return BadRequest(stockAPDto.Message);
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Use specific exception handling
            }
        }
    }
}