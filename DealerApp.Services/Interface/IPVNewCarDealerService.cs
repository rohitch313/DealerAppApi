
using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPVNewCarDealerService
    {
        Task<IEnumerable<PV_NewCarDealerDTO>> GetNewCarDealerSupportAsync();
        Task<PV_NewCarDealerDTO> PostNewCarDealerAsync(PV_NewCarDealerDTO pv_cardealerDTO, int userId);
    }
}
