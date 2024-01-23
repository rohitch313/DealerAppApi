

using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPVAggregatorService
    {
        Task<IEnumerable<PV_AggregatorDTO>> GetAggregatorSupportAsync();
        Task<PV_AggregatorDTO> PostAggregatorSupportAsync(PV_AggregatorDTO pv_aggregatorDTO, int userId);
    }
}
