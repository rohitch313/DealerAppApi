using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerAPP.Service.Services
{
    public class AccountDetailsService : IAccountDetailsService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AccountDetailsService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// This is service to get Account detail of User
        /// </summary>
     
        public async Task<IEnumerable<AccountDetailsDTO>> GetAccountDetails(int userId)
        {
            try
            {
                var accountDetails = await _db.AccountDetailstbl
                    .Where(a => a.UserInfoId == userId)
                    .ToListAsync();

                if (accountDetails == null || accountDetails.Count == 0)
                {
                    
                    return null;
                }

                // Use AutoMapper to map the entities to DTO
                var accountDetailsDto = _mapper.Map<IEnumerable<AccountDetailsDTO>>(accountDetails);

                return accountDetailsDto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching account details.", ex);
            }
        }

    }
}
