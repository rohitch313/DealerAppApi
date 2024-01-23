
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class AccountDetailsAPIController : ControllerBase
    {
        private readonly IAccountDetailsService _accountDetailsService;

        public AccountDetailsAPIController(IAccountDetailsService accountDetailsService)
        {
            _accountDetailsService = accountDetailsService;
        }
        /// <summary>
        /// This is the controller to Get Account Details of Login User
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public async Task<IActionResult> GetAccountDetails()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdString, out int userId))
                {
                    var accountDetails = await _accountDetailsService.GetAccountDetails(userId);

                    if (accountDetails == null)
                    {
                        return NotFound("No account details found");
                    }
                    return Ok(accountDetails);
                }
                else
                {
                    return BadRequest("Invalid user ID");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }




    }
}