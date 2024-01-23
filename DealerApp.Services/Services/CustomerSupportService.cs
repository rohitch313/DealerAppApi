using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerApp.Service.Services
{
    public class CustomerSupportService : ICustomerSupportService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CustomerSupportService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// This Service method is used to get  Customer Support Detail 
        /// </summary>
        
        public async Task<IEnumerable<CustomerSupportDTO>> GetCustomerSupportAsync()
        {
            try
            {
                var customers = await _db.CustomerSupporttbl.ToListAsync();

                if (customers == null || customers.Count == 0)
                {
                    return null;
                }

                // Use AutoMapper to map the entities to DTO
                var customerDtos = _mapper.Map<IEnumerable<CustomerSupportDTO>>(customers);

                return customerDtos;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate for your application
                // You might want to throw a custom exception or return a specific error message
                throw new ApplicationException("An error occurred while fetching customer support data.", ex);
            }
        }

        // Add other methods implementing the business logic for customer support
    }
}
