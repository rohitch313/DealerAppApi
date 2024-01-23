using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Model.Models;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.Service.Services
{
    public class PVAggregatorService : IPVAggregatorService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PVAggregatorService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PV_AggregatorDTO>> GetAggregatorSupportAsync()
        {
            try
            {
                var aggreator = await _db.PV_Aggregatorstbl.ToListAsync();

                if (aggreator == null || aggreator.Count == 0)
                {
                    // Return an empty list if no data is found
                    return new List<PV_AggregatorDTO>();
                }

                // Use AutoMapper to map the entities to DTO
                var aggreatorDto = _mapper.Map<IEnumerable<PV_AggregatorDTO>>(aggreator);

                return aggreatorDto;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                // Log the exception or handle it as appropriate for your application
                throw; // Rethrow the exception for now, replace with appropriate handling
            }
        }

        /// <summary>
        /// this method is used to post purchase of vehicle  in aggregate
        /// </summary>
        /// <param name="pv_AggregatorDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<PV_AggregatorDTO> PostAggregatorSupportAsync(PV_AggregatorDTO pv_AggregatorDTO, int userId)
        {
            try
            {
                // Set UserId in the DTO before mapping to the entity
                pv_AggregatorDTO.UserInfoId = userId;

                var pvAggregatorModel = _mapper.Map<PV_Aggregator>(pv_AggregatorDTO);

                _db.PV_Aggregatorstbl.Add(pvAggregatorModel);
                await _db.SaveChangesAsync();

                var createdDto = _mapper.Map<PV_AggregatorDTO>(pvAggregatorModel);

                return createdDto;
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