
using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.Service.Services
{
    public class PVAMakeService : IPVAMakeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PVAMakeService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// Thisis used to get list of car maker
        /// </summary>

        public async Task<IEnumerable<PVA_MakeDTO>> GetMakeDetailsAsync()
        {
            try
            {
                var makeDetails = await _db.PVA_Maketbl.ToListAsync();

                if (!makeDetails.Any())
                {
                    return null;
                }
                var makeDetailsDto = _mapper.Map<IEnumerable<PVA_MakeDTO>>(makeDetails);

                return makeDetailsDto;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw; 
            }
        }
    }
}