

using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class CustomerSupportAPIController : ControllerBase
    {
        private readonly ICustomerSupportService _customerSupportService;

        public CustomerSupportAPIController(ICustomerSupportService customerSupportService)
        {
            _customerSupportService = customerSupportService;
        }
        /// <summary>
        /// This is the Method to get Customer Support Detail
        /// </summary>

        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  async Task<IActionResult> GetCustomerSupport() 
        {
            try
            {
              var customer = await _customerSupportService.GetCustomerSupportAsync();
                if (customer == null)
                {
                    return NotFound("No account details found");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}