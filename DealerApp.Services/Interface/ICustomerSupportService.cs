

using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface ICustomerSupportService
    {
        Task<IEnumerable<CustomerSupportDTO>> GetCustomerSupportAsync();
    }
}
