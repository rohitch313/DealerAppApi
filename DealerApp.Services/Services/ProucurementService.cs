using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Model.Models;
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
    public class ProucurementService : IProucurementService
    {
        private readonly ApplicationDbContext _db;


        public ProucurementService(ApplicationDbContext db)
        {
            _db = db;

        }
        public async Task<ResponseDto> GetFiltersAsync()
        {
            try
            {
                // ... (fetch filters logic)

                var filter = await _db.ProcurementFilters.ToListAsync();

                return new ResponseDto
                {
                    Success = true,
                    Data = filter
                };
            }
            catch (Exception ex)
            {
                // ... (logCommonResponseModelging and exception handling)
                return new ResponseDto
                {
                    Success = false,
                    Message = $"Error fetching filters:{ex.Message}",
                    // Optionally include error details for debugging
                };
            }
        }


        public async Task<ResponseDto> GetProcurementsAsync(int? filterId, string userId)
        {
            try
            {
                int userIdInt;
                if (!int.TryParse(userId, out userIdInt))
                {
                    return new ResponseDto { Success = false, Message = "Invalid user ID" };
                }

                IQueryable<ProcDetails> procurementQuery = _db.procDetails
                    .Include(p => p.Payment)
                    .ThenInclude(pay => pay.Car)
                    .Where(p => p.Payment.Car.UserId == userIdInt);

                if (!filterId.HasValue)
                {
                   
                    var allProcurements = await procurementQuery
                        .Select(p => new ProcurementDto
                        {
                            FilterId = p.FilterId,
                            PurchaseId = p.CarId,
                            CarName = p.Payment.CarName,
                            Variant = p.Payment.Variant,
                            Amount_due = p.Due_Amount,
                            Amount_paid = p.Paid_Amount,
                            Facility_Availed = p.Facility_Availed,
                            Invoice_Charges = p.Invoice_Charges,
                            Processing_charges = p.ProcessingCharges,

                            // Map properties from ProcDetails to ProcurementResponse
                        })
                        .ToListAsync();

                    return new ResponseDto { Success = true, Data = allProcurements };
                }

              
                var filteredProcurements = await procurementQuery
                    .Where(p => p.FilterId == filterId)
                    .Select(p => new ProcurementDto
                    {
                        FilterId = p.FilterId,
                        PurchaseId = p.CarId,
                        CarName = p.Payment.CarName,
                        Variant = p.Payment.Variant,
                        Amount_due = p.Due_Amount,
                        Amount_paid = p.Paid_Amount,
                        Facility_Availed = p.Facility_Availed,
                        Invoice_Charges = p.Invoice_Charges,
                        Processing_charges = p.ProcessingCharges,
                        // Map properties from ProcDetails to ProcurementResponse
                    })
                    .ToListAsync();

                if (!filteredProcurements.Any())
                {
                    return new ResponseDto { Success = false, Message = "No procurements found" };
                }

                return new ResponseDto { Success = true, Data = filteredProcurements };
            }
            catch (Exception ex)
            {
               return new ResponseDto { Success = false, Message = $"Error fetching procurements:{ex.Message}" };
            }
        }
        public async Task<ResponseDto> GetProcurementStatusAsync(string userId)
        {
            try
            {
                int userIdInt;
                if (!int.TryParse(userId, out userIdInt))
                {
                    return new ResponseDto { Success = false, Message = "Invalid user ID" };
                }

              
                var procurementStatus = await _db.procDetails
                    .Include(c => c.Payment)
                    .ThenInclude(c => c.Car)
                    .Where(c => c.Status != null && c.Payment.Car.UserId == userIdInt)
                    .Select(c => new ProcurementStatusDto
                    {
                        CarName = c.Payment.CarName,
                        Variant = c.Payment.Variant,
                        PurchaseId = c.CarId,
                        Status = c.Status,
                        Purchased_Amount = c.Purchased_Amount
                    })
                    .ToListAsync();

                return new ResponseDto { Success = true, Data = procurementStatus };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching procurement status: {ex.Message}");
                return new ResponseDto { Success = false, Message = "Error fetching procurement status" };
            }
        }
        public async Task<ResponseDto> GetProcurementClosedAsync(string userId)
        {
            try
            {
                
                if (!int.TryParse(userId, out int userIdInt))
                {
                    return new ResponseDto { Success = false, Message = "Invalid user ID" };
                }

             

                var procurementClosed = await _db.procDetails
                    .Include(c => c.Payment)
                    .ThenInclude(c => c.Car)
                    .Where(c => c.ClosedOn != null && c.Payment.Car.UserId == userIdInt)
                    .Select(c => new ProcurementColsedDto
                    {
                        CarName = c.Payment.CarName,
                        Variant = c.Payment.Variant,
                        Amount_paid = c.Paid_Amount,
                        ColsedOn = c.ClosedOn,
                        PurchaseId = c.CarId
                    })
                    .ToListAsync();

                return new ResponseDto { Success = true, Data = procurementClosed };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching procurement closed: {ex.Message}");
                return new ResponseDto { Success = false, Message = "Error fetching procurement closed" };
            }
        }
    }
}



  
