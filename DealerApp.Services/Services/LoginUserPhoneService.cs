using AutoMapper;
using Azure;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Model.Models;
using DealerApp.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace DealerApp.Service.Services
{
    public class LoginUserPhoneService : ILoginUserPhoneService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginUserPhoneService(ApplicationDbContext db, IMapper mapper, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> GenerateOTPAsync(string phoneNumber)
        {
            try
            {
                //if (string.IsNullOrEmpty(phoneNumber))
                //{
                //    throw new ArgumentException("Phone number is required to generate OTP.", nameof(phoneNumber));
                //}

                // Check if the user with the given phone number exists in the database
                var user = await _db.Userstbl.FirstOrDefaultAsync(u => u.Phone == phoneNumber);

                if (user == null)
                {
                    // Throw a more general exception
                    throw new InvalidOperationException("User not found.");
                }

                // Generate a 4-digit OTP
                int otp = GenerateOTP();

                // Set the OTP and its expiry in the UserInfo table
                user.OTP = otp;
                // Expiry set to 1 minute from the current time
                user.OTPExpiry = DateTime.UtcNow.AddMinutes(1);
                await _db.SaveChangesAsync();

                // Return the OTP for the user to enter on the next page
                return otp;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException($"Error in GenerateOTPAsync service: {ex.Message}", ex);
            }
        } 
        public async Task<IActionResult> ResendOTPAsync(string phoneNumber)
        {
            try
            {


                // Check if the user with the given phone number exists in the database
                var user = await _db.Userstbl.FirstOrDefaultAsync(u => u.Phone == phoneNumber);

                if (user == null )
                {
                    return null;
                }

                // Check if the previous OTP expired
                if (IsOTPOutdated(user.OTPExpiry))
                {
                    // Generate a new OTP and update user information
                    int newOTP = GenerateOTP();
                    user.OTP = newOTP;
                    user.OTPExpiry = DateTime.UtcNow.AddMinutes(1); // Set a new expiration time

                    // Save changes to the database
                    await _db.SaveChangesAsync();

                    // Send the new OTP via SMS or any other method (not shown here)

                    return new OkObjectResult("New OTP sent successfully.");
                }

                return new BadRequestObjectResult("Previous OTP is still valid.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return new ObjectResult($"An error occurred: {ex.Message}")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        private bool IsOTPOutdated(DateTime? otpExpiration)
        {
            if (otpExpiration.HasValue && otpExpiration.Value < DateTime.UtcNow)
            {
                return true; // OTP has expired
            }
            return false; // OTP is still valid
        }

        private int GenerateOTP()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }


        public async Task<IActionResult> VerifyOTPAsync(string phoneNumber, int enteredOTP)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber) || enteredOTP == 0)
                {
                    return new BadRequestObjectResult("Phone number and OTP are required for verification.");
                }

                var user =await _db.Userstbl.FirstOrDefaultAsync(u => u.Phone == phoneNumber);

                if (user == null)
                {
                    return new NotFoundObjectResult("User not found.");
                }

                if (user.OTP == enteredOTP && user.OTPExpiry > DateTime.UtcNow)
                {
                    user.OTP = null;
                    user.OTPExpiry = DateTime.MinValue;

                    if (user.Active)
                    {
                        _db.SaveChanges();

                        string token = CreateToken(user);
                        var refreshToken = GenerateRefreshToken();
                        SetRefreshToken(refreshToken, user);

                        return new OkObjectResult(token);
                    }
                    else
                    {
                        user.OTP = null;
                        user.OTPExpiry = DateTime.MinValue;
                        _db.SaveChanges();

                        return new BadRequestObjectResult("User is not active. Token cannot be generated.");
                    }
                }

                user.OTP = null;
                user.OTPExpiry = DateTime.MinValue;
                _db.SaveChanges();

                return new BadRequestObjectResult("Invalid OTP or OTP has expired");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"An error occurred: {ex.Message}")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(30),
                Created = DateTime.UtcNow
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken refreshToken, UserInfo user)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = refreshToken.Expires,
                };

                httpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
            }

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;
            _db.SaveChanges();
        }
        private string CreateToken(UserInfo userInfo)
        {
            List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
            new Claim(ClaimTypes.Name, userInfo.UserName)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsettings:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                audience: "http://localhost:5090",
                signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        public async Task<ResponseDto> LogoutAsync(int userId)
        {
            try
            {
                var user = await _db.Userstbl.FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    user.RefreshToken = null;
                    user.TokenCreated = DateTime.MinValue;
                    user.TokenExpires = DateTime.MinValue;
                    await _db.SaveChangesAsync();

                    // Clear the token cookie on the client-side (if applicable)
                    // ...

                    return new ResponseDto { Success = true, Message = "Logged out successfully" };
                }

                return new ResponseDto { Success = false, Message = "User not found" };
            }
            catch (Exception ex)
            {
                // Log the error
                return new ResponseDto { Success = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
        public async Task<ResponseDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _db.Userstbl.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

                if (user == null)
                {
                    return new ResponseDto { Success = false, Message = "Invalid Refresh Token." };
                }
                else if (user.TokenExpires < DateTime.Now)
                {
                    return new ResponseDto { Success = false, Message = "Token expired." };
                }

                string token = CreateToken(user);
                var newRefreshToken = GenerateRefreshToken();
                SetRefreshToken(newRefreshToken, user);
                await _db.SaveChangesAsync();

                return new ResponseDto { Success = true, Token = token };
            }
            catch (Exception ex)
            {
                // Log the error
                return new ResponseDto { Success = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
            public async Task<ResponseDto> RegisterUserAsync(string phone)
            {
                try
                {


                    var existingUser = await _db.Userstbl.FirstOrDefaultAsync(u => u.Phone == phone);
                    if (existingUser != null)
                    {
                        return new ResponseDto { Success = false, Message = "Phone number already exists" };
                    }

                    var newUser = new UserInfo
                    {
                        Phone = phone,
                        // Set other properties as needed
                    };

                    _db.Userstbl.Add(newUser);
                    await _db.SaveChangesAsync();

                    int otp = GenerateOTP();
                    newUser.OTP = otp;
                    newUser.OTPExpiry = DateTime.UtcNow.AddMinutes(1);
                    await _db.SaveChangesAsync();

                    return new ResponseDto { Success = true, OTP = otp };
                }
                catch (Exception ex)
                {
                    // Log the error
                    return new ResponseDto { Success = false, Message = $"An error occurred: {ex.Message}" };
                }
            }
        public async Task<ResponseDto> AddAdditionalUserDetailsAsync(string phone, UserAdditionalDetailsDto additionalDetails)
        {
            try
            {


                var existingUser = await _db.Userstbl.FirstOrDefaultAsync(u => u.Phone == phone);
                if (existingUser == null)
                {
                    return new ResponseDto { Success = false, Message = "Phone number not found" };
                }

                existingUser.UserName = additionalDetails.UserName;
                existingUser.UserEmail = additionalDetails.UserEmail;
                existingUser.SId = additionalDetails.SId;

                await _db.SaveChangesAsync();

                if (existingUser.Active)
                {
                    string token = CreateToken(existingUser);
                    var refreshToken = GenerateRefreshToken();
                    SetRefreshToken(refreshToken, existingUser);
                    await _db.SaveChangesAsync();
                    return new ResponseDto { Success = true, Token = token };
                }
                else
                {
                    return new ResponseDto { Success = true, Message = "User details updated but token not generated because the user is not active." };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                return new ResponseDto { Success = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
        public async Task<ResponseDto> VerifyOTPSignupAsync(string phoneNumber, int enteredOTP)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber) || enteredOTP == null)
                {
                    return new ResponseDto { Success = false, Message = "Phone number and OTP are required for verification." };
                }

                var user = await _db.Userstbl.FirstOrDefaultAsync(u => u.Phone == phoneNumber);

                if (user == null)
                {
                    return new ResponseDto { Success = false, Message = "User not found." };
                }

                if (user.OTP == enteredOTP && user.OTPExpiry > DateTime.UtcNow)
                {
                    user.OTP = null;
                    user.OTPExpiry = DateTime.MinValue;
                    await _db.SaveChangesAsync();
                    return new ResponseDto { Success = true };
                }
                else
                {
                    user.OTP = null;
                    user.OTPExpiry = DateTime.MinValue;
                    await _db.SaveChangesAsync();
                    return new ResponseDto { Success = false, Message = "Invalid OTP or OTP has expired" };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                return new ResponseDto { Success = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
            public async Task<UserStatusDTO> GetUserStatusAsync(string phoneNumber)
            {
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    throw new ArgumentNullException(nameof(phoneNumber));
                }

                var user = await _db.Userstbl.FirstOrDefaultAsync(u => u.Phone == phoneNumber);

                if (user == null)
                {
                    return null; // Or throw a specific exception for user not found
                }

                return new UserStatusDTO
                {
                    Active = user.Active,
                    Rejected = user.Rejected
                };
            }
        }
    }
    
    


