using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.Service.Services
{
    public class PVAYearOfRegService : IPVAYearOfRegService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PVAYearOfRegService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// this is method is used to get year of registration of car from database
        /// </summary>

        public async Task<IEnumerable<PVA_YearOfRegDTO>> GetYearOfRegDetailsAsync()
        {
            try
            {
                var yearOfRegDetails = await _db.PVA_YearOfRegtbl.ToListAsync();

                if (yearOfRegDetails == null || yearOfRegDetails.Count == 0 || !yearOfRegDetails.Any())
                {
                    // Return an empty list if no data is found
                    return null;
                }

                // Use AutoMapper to map the entities to DTO
                var yearOfRegDetailsDto = _mapper.Map<IEnumerable<PVA_YearOfRegDTO>>(yearOfRegDetails);

                return yearOfRegDetailsDto;
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
