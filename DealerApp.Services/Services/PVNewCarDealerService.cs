using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Model.Models;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.Service.Services
{
    public class PVNewCarDealerService : IPVNewCarDealerService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PVNewCarDealerService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PV_NewCarDealerDTO>> GetNewCarDealerSupportAsync()
        {
            try
            {
                var carDealer = await _db.PV_NewCarDealerstbl.ToListAsync();

                if (carDealer == null || carDealer.Count == 0 || !carDealer.Any())
                {
                    // Return an empty list if no data is found
                    return new List<PV_NewCarDealerDTO>();
                }

                // Use AutoMapper to map the entities to DTO
                var carDealerDto = _mapper.Map<IEnumerable<PV_NewCarDealerDTO>>(carDealer);

                return carDealerDto;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                // Log the exception or handle it as appropriate for your application
                throw; // Rethrow the exception for now, replace with appropriate handling
            }
        }
        /// <summary>
        /// this method is used to  post  purchase of vichele  in New Car Dealer  
        /// </summary>
        /// <param name="pv_cardealerDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PV_NewCarDealerDTO> PostNewCarDealerAsync(PV_NewCarDealerDTO pv_cardealerDTO, int userId)
        {
            try
            {
                // Set UserId in the DTO before mapping to the entity
                pv_cardealerDTO.UserInfoId = userId;

                var pv_cardealerModel = _mapper.Map<PV_NewCarDealer>(pv_cardealerDTO);

                _db.PV_NewCarDealerstbl.Add(pv_cardealerModel);
                await _db.SaveChangesAsync();

                var pv_carDtoModel = _mapper.Map<PV_NewCarDealerDTO>(pv_cardealerModel);

                return pv_carDtoModel;
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