using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Services
{
    public class UserAccountService:IUserAccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;


        public UserAccountService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            
        }

        public async Task<ResponseDto> GetUserAccountSupportAsync(string userId)
        {
            try
            {
                int userIdInt;
                if (!int.TryParse(userId, out userIdInt))
                {
                    return new ResponseDto { Success = false, Message = "Invalid user ID" };
                }

                var userInfoDetails = await _db.Userstbl
                    .Where(user => user.Id == userIdInt)
                    .ToListAsync();

                if (userInfoDetails == null || userInfoDetails.Count == 0)
                {
                    return new ResponseDto { Success = false, Message = "No user details found" };
                }

                var userInfoDto = _mapper.Map<IEnumerable<UserAccountDTO>>(userInfoDetails);
                return new ResponseDto { Success = true, Data = userInfoDto };
            }
            catch (Exception ex)
            {
               
                return new ResponseDto { Success = false, Message = $"Error fetching user account support:{ex.Message}" };
            }
        }
    }
}

