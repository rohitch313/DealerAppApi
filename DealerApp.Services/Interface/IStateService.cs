
using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{ 
    public interface IStateService
    {
        Task<IEnumerable<StateDTO>> GetStateDetailsAsync();
    }
}
