using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Model.Models;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;


namespace DealerApp.Service.Services
{
    public class PVOpenMarketService : IPVOpenMarketService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PVOpenMarketService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PV_OpenMarketDTO>> GetOpenMarketSupportAsync()
        {
            try
            {
                var openMarket = await _db.PV_OpenMarketstbl.ToListAsync();

                if (openMarket == null || openMarket.Count == 0 || !openMarket.Any())
                {
                    // Return an empty list if no data is found
                    return null;
                }

                // Use AutoMapper to map the entities to DTO
                var openMarketDto = _mapper.Map<IEnumerable<PV_OpenMarketDTO>>(openMarket);

                return openMarketDto;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                // Log the exception or handle it as appropriate for your application
                throw; // Rethrow the exception for now, replace with appropriate handling
            }
        }
        /// <summary>
        /// this method is used to  post  purchase of vichele  in open market        /// </summary>
        /// <param name="pV_OpenmarketDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<PV_OpenMarketDTO> PostOpenMarketSupportAsync(PV_OpenMarketDTO pV_OpenmarketDTO, int userId)
        {
            try
            {
                // Set UserId in the DTO before mapping to the entity
                pV_OpenmarketDTO.UserInfoId = userId;

                var pV_openmarketModel = _mapper.Map<PV_OpenMarket>(pV_OpenmarketDTO);

                _db.PV_OpenMarketstbl.Add(pV_openmarketModel);
                await _db.SaveChangesAsync();

                var pV_openmarketDtoModel = _mapper.Map<PV_OpenMarketDTO>(pV_openmarketModel);

                return pV_openmarketDtoModel;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                // Log the exception or handle it as appropriate for your application
                throw; // Rethrow the exception for now, replace with appropriate handling
            }
        }
    }
}