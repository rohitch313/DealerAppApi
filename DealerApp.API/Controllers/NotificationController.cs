using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealerApp.API.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public

    async Task<ActionResult<IEnumerable<NotificationDTO>>> GetNotificationSupport()
        {
            try
            {
                var response = await _notificationService.GetNotificationSupportAsync();

                if (response.Success)
                {
                    if (response.Data == null)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(response.Data);
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error:{ex.Message}");
            }
        }
    }
}
