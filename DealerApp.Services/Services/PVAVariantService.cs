using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.Service.Services
{
    public class PVAVariantService : IPVAVariantService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        /// <summary>
        /// this is method is used to get variant of car from database
        /// </summary>
        public PVAVariantService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PVA_VariantDTO>> GetVariantDetailsAsync()
        {
            try
            {
                var variantDetails = await _db.PVA_Varianttbl.ToListAsync();

                if (variantDetails == null || variantDetails.Count == 0 || !variantDetails.Any())
                {
                    // Return an empty list if no data is found
                    return null;
                }

                // Use AutoMapper to map the entities to DTO
                var variantDetailsDto = _mapper.Map<IEnumerable<PVA_VariantDTO>>(variantDetails);

                return variantDetailsDto;
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