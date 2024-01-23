using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Services
{
    public class StockPurposeService : IStockPurposeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public StockPurposeService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetStockPurposeDetailsAsync()
        {
            try
            {
                var stockAPdetails = await _db.StockAudit_Purposestbl.ToListAsync();

                if (stockAPdetails == null || stockAPdetails.Count == 0)
                {
                    return new ResponseDto { Success = false, Message = "No stock purpose found for the user" };
                }

                var stockAPDto = _mapper.Map<IEnumerable<StockAudit_PurposeDTO>>(stockAPdetails);

                return new ResponseDto { Success = true, Data = stockAPDto };
            }
            catch (Exception)
            {
                return new ResponseDto { Success = false, Message = "Error fetching user Stock purpose" };
            }
        }
    }
}
