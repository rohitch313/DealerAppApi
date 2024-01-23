using AutoMapper;

using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.Service.Services
{
    public class PVAModelService : IPVAModelService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PVAModelService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// this is method is used to get model of car from database
        /// </summary>
        public async Task<IEnumerable<PVA_ModelDTO>> GetModelDetailsAsync()
        {
            try
            {
                var modelDetails = await _db.PVA_Modeltbl.ToListAsync();

                if (modelDetails == null || modelDetails.Count == 0)
                {
                    // Return an empty list if no data is found
                    return null;
                }

                // Use AutoMapper to map the entities to DTO
                var modelDetailsDto = _mapper.Map<IEnumerable<PVA_ModelDTO>>(modelDetails);

                return modelDetailsDto;
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
