using DealerApp.Dtos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Interface
{
    public interface ILoginUserPhoneService
    {
        Task<int> GenerateOTPAsync(string phoneNumber);
        Task<IActionResult> ResendOTPAsync(string phoneNumber);
        Task<IActionResult> VerifyOTPAsync( string phoneNumber, int enteredOTP);
        Task<ResponseDto> LogoutAsync(int userId);
        Task<ResponseDto> RefreshTokenAsync(string refreshToken);
        Task<ResponseDto> RegisterUserAsync(string phone);
        Task<ResponseDto> AddAdditionalUserDetailsAsync(string phone, UserAdditionalDetailsDto additionalDetails);
        Task<ResponseDto> VerifyOTPSignupAsync(string phoneNumber, int enteredOTP);
        Task<UserStatusDTO> GetUserStatusAsync(string phoneNumber);


    }
}
