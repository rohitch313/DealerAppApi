using DealerApp.Dtos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Interface
{
    public interface IStockPurposeService
    {
        Task<ResponseDto> GetStockPurposeDetailsAsync();
    }
}
