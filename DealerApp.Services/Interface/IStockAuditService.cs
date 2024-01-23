using DealerApp.Dtos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Interface
{
    public interface IStockAuditService
    {
        Task<ResponseDto> GetUpcomingAuditsAsync(string userId);
        Task<ResponseDto> GetPendingAuditsAsync(string userId);
        Task<ResponseDto> GetStockStatusAsync(string userId);
        Task<ResponseDto> GetUserAddressesAsync(string userId);

    }
}
