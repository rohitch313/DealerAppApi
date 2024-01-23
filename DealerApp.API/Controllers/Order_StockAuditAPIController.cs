using AutoMapper;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using DealerApp.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.API.Controllers
{


    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class Order_StockAuditAPIController : ControllerBase
    {
        private readonly IOrderStockAuditService _orderStockAuditService;

        public Order_StockAuditAPIController(IOrderStockAuditService orderStockAuditService)
        {
            _orderStockAuditService = orderStockAuditService;
        }

        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockSupport()
        {
            try
            {
                var stock = await _orderStockAuditService.GetStockSupportAsync();
                if (stock == null)
                {
                    return NotFound("No stock found");
                }
                return Ok(stock);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Order_StockAuditDTO>> PostStockSupport(Order_StockAuditDTO stockAuditDTO)
        {
            try
            {
                var result = await _orderStockAuditService.PostStockSupportAsync(stockAuditDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500); // Return 500 without exposing internal server error message
            }
        }
    }
}
  
