

using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPVOpenMarketService
    {
        Task<IEnumerable<PV_OpenMarketDTO>> GetOpenMarketSupportAsync();
        Task<PV_OpenMarketDTO> PostOpenMarketSupportAsync(PV_OpenMarketDTO pV_openmarketDTO, int userId);
    }
}
