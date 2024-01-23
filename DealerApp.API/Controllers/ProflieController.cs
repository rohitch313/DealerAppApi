using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class ProflieController : Controller
    {

            private readonly IProfileInformationService _profileService;

            public ProflieController(IProfileInformationService profileService)
            {
                _profileService = profileService;
            }

            [HttpGet]
            [Authorize]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]


            public async Task<IActionResult> GetProfileSupport()
            {
                try
                {
                    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var response = await _profileService.GetProfileSupportAsync(userIdString);
                    if (response.Success)
                    {
                        return Ok(response.Data);
                    }
                    else
                    {
                        return BadRequest(response.Message);
                    }

                }
                catch (Exception ex)
                {

                    return StatusCode(500, ex.Message);
                }
            }
        }
    }

  