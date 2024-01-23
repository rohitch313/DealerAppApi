
using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPVAYearOfRegService
    {
        Task<IEnumerable<PVA_YearOfRegDTO>> GetYearOfRegDetailsAsync();
    }
}
