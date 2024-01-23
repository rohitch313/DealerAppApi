using DealerApp.Dtos.DTO;
using DealerApp.Model.Models;
using DealerApp.Service.Interface;
using DealerApp.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DealerApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LoginUserPhoneController : Controller
    {
        private readonly ILoginUserPhoneService _userPhoneService;
        private readonly ILogger<LoginUserPhoneController> _logger;

        public LoginUserPhoneController(ILoginUserPhoneService userPhoneService, ILogger<LoginUserPhoneController> logger )
        {
            _userPhoneService = userPhoneService;
            _logger = logger;
        }

        [HttpPost("generateotp")]
        public async Task<IActionResult> GenerateOTP(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    return BadRequest("Phone number is required to generate OTP.");
                }
                int otp = await _userPhoneService.GenerateOTPAsync(phoneNumber);

                return Ok(otp); // Modify this as needed
            }
           
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpPost("resend-otp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResendOTPAsync(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    return BadRequest("Phone number is required to generate OTP.");
                }

                var newOTP = await _userPhoneService.ResendOTPAsync(phoneNumber);

                if (newOTP == null)
                {
                    return NotFound("User not found.");
                }


                _logger.LogInformation("New OTP sent successfully to phone number {phoneNumber}", phoneNumber); 
                return Ok("New OTP sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resending OTP."); // Log error with details
                return StatusCode(500, "An error occurred. Please try again later.");
            }
        }
        [HttpPost("verifyotp")]

        public async Task<IActionResult> VerifyOTP(string phoneNumber, int enteredOTP)
        {
            

                return await _userPhoneService.VerifyOTPAsync(phoneNumber, enteredOTP);
            }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int userIdInt))
            {
                var logoutResult = await _userPhoneService.LogoutAsync(userIdInt);

                if (logoutResult.Success)
                {
                    Response.Cookies.Delete("refreshToken");
                    return Ok(logoutResult.Message);
                }
                else
                {
                    // Handle potential errors returned from the service
                    return BadRequest(logoutResult.Message);
                }
            }

            return BadRequest("Invalid or missing user ID");
        }



        [HttpPost("refresh-token")]

        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var refreshTokenResult = await _userPhoneService.RefreshTokenAsync(refreshToken);

            if (refreshTokenResult.Success)
            {
                // Update the refresh token cookie on the client-side
                // ...
                return Ok(refreshTokenResult.Token);
            }
            else
            {
                return Unauthorized(refreshTokenResult.Message);
            }
        }
        [HttpPost("Register")]
        public

async Task<IActionResult> RegisterUser(string phone)
        {
            var registerResult = await _userPhoneService.RegisterUserAsync(phone);

            if (registerResult.Success)
            {
                return Ok(registerResult.OTP);
            }
            else
            {
                return BadRequest(registerResult.Message); // Or Conflict if appropriate
            }
        }




        [HttpPost("AdditionalDetails")]
        public async Task<IActionResult> AddAdditionalUserDetails([FromBody] UserAdditionalDetailsDto additionalDetails, string phone)
        {
            var result = await _userPhoneService.AddAdditionalUserDetailsAsync(phone, additionalDetails);

            if (result.Success)
            {
                if (result.Token != null)
                {
                    return Ok(result.Token);
                }
                else
                {
                    return Ok(result.Message);
                }
            }
            else
            {
                return BadRequest(result.Message); // Or NotFound if appropriate
            }
        }
        [HttpPost("verifyotpSignup")]
        public async Task<IActionResult> VerifyOTPSignup(string phoneNumber, int enteredOTP)
        {
            var verifyResult = await _userPhoneService.VerifyOTPSignupAsync(phoneNumber, enteredOTP);

            if (verifyResult.Success)
            {
                return Ok(); // Or return a more specific success response
            }
            else
            {
                return BadRequest(verifyResult.Message);
            }
        }


        [HttpGet("userstatus")]
        public async Task<IActionResult> GetUserStatus(string phoneNumber)
        {
            var userStatus = await _userPhoneService.GetUserStatusAsync(phoneNumber);

            if (userStatus == null)
            {
                return NotFound("User not found.");
            }

            return Ok(userStatus);
        }
    }

}




