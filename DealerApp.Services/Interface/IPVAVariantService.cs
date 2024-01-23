
using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPVAVariantService
    {
        Task<IEnumerable<PVA_VariantDTO>> GetVariantDetailsAsync();
    }
}
