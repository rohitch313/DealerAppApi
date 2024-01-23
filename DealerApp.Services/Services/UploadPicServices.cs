using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Services
{
    public class UploadPicServices:IUploadPicServices
    {
        private readonly ApplicationDbContext _db;
        

        public UploadPicServices(ApplicationDbContext db)
        {
            _db = db;

        }

        public async Task<ResponseDto> UploadPicAsync(UploadPic_StockAuditDTO request)
        {
            try
            {


                var existingStockAudit = await _db.StockAudits.FirstOrDefaultAsync(s => s.Id == request.Id);

                if (existingStockAudit == null)
                {
                    return new ResponseDto
                    {
                        Success = false,
                        Message = $"Stock Audit with ID '{request.Id}' not found"
                    };
                }

                // Update the properties of the existing stock audit
                existingStockAudit.image1 = request.image1;
                existingStockAudit.image2 = request.image2;
                existingStockAudit.image3 = request.image3;

                // Set varified based on the condition
                existingStockAudit.varified = !string.IsNullOrEmpty(request.image1)
                    && !string.IsNullOrEmpty(request.image2)
                    && !string.IsNullOrEmpty(request.image3);

                // Save changes to the database
                await _db.SaveChangesAsync();

                return new ResponseDto
                {
                    Success = true,
                    Message = "Upload successful"
                };
            }
            catch (Exception ex)
            {
                
                return new ResponseDto
                {
                    Success = false,
                    Message = $"Internal Server Error: {ex.Message}"
                };
            }
        }
    }
}
