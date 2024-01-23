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
    public class ProfileInformationService:IProfileInformationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
       

        public ProfileInformationService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }

        public async Task<ResponseDto> GetProfileSupportAsync(string userId)
        {
            try
            {
                int userIdInt;
                if (!int.TryParse(userId, out userIdInt))
                {
                    return new ResponseDto { Success = false, Message = "Invalid user ID" };
                }

                var profiles = await _db.ProfileInformationtbl
                    .Where(p => p.UserInfoId == userIdInt)
                    .ToListAsync();

                if (profiles == null || profiles.Count == 0)
                {
                    return new ResponseDto { Success = false, Message = "No account details found" };
                }

                var profileDto = _mapper.Map<IEnumerable<ProfileInformationDTO>>(profiles);
                return new ResponseDto { Success = true, Data = profileDto };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching profile support: {ex.Message}");
                return new ResponseDto { Success = false, Message = "Error fetching profile support" };
            }
        }
    }
}

