using DealerApp.Dtos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Interface
{
    public interface IProucurementService
    {
        Task<ResponseDto> GetFiltersAsync();
        Task<ResponseDto> GetProcurementsAsync(int? filterId, string userId);
        Task<ResponseDto> GetProcurementStatusAsync(string userId);

        Task<ResponseDto> GetProcurementClosedAsync(string userId);
    }
}
