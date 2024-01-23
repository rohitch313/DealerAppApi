using AutoMapper;



using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.Service.Services
{
    public class StateService : IStateService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public StateService(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }
        /// <summary>
        /// This method is used to get list  state from database 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StateDTO>> GetStateDetailsAsync()
        {
            try
            {
                var statedetails = await _db.Statetbl.ToListAsync();

                if (statedetails == null || statedetails.Count == 0||!statedetails.Any())
                {
                    // Return an empty list if no data is found
                    return null;
                }

                // Use AutoMapper to map the entities to DTO
                var statesDto = _mapper.Map<IEnumerable<StateDTO>>(statedetails);

                return statesDto;
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

