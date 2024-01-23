
using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IAccountDetailsService
    {
        Task<IEnumerable<AccountDetailsDTO>> GetAccountDetails(int userId);

    }
}
