

using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPVAMakeService
    {
        Task<IEnumerable<PVA_MakeDTO>> GetMakeDetailsAsync();
    }
}
