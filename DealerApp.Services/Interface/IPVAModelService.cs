
using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPVAModelService
    {
        Task<IEnumerable<PVA_ModelDTO>> GetModelDetailsAsync();
    }
}
