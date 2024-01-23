using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DealerApp.Service.Services
{
    public class StockAuditService : IStockAuditService
    {
        private readonly ApplicationDbContext _db;


        public StockAuditService(ApplicationDbContext db, ILogger<StockAuditService> logger)
        {
            _db = db;

        }

        public async Task<ResponseDto> GetUpcomingAuditsAsync(string userId)
        {
            try
            {
                int userIdInt;
                if (!int.TryParse(userId, out userIdInt))
                {
                    return new ResponseDto { Success = false, Message = "Invalid user ID" };
                }

                var currentDate = DateTime.UtcNow;

                var userCars = await _db.Cars.Where(c => c.UserId == userIdInt).ToListAsync();
                var carIds = userCars.Select(car => car.CarId).ToList();

                var upcomingAudits = await _db.StockAudits
                    .Include(a => a.Car)
                    .Where(p => carIds.Contains(p.CarId))
                    .ToListAsync();

                var filterUpcomingAudit = upcomingAudits.Where(a => (a.AuditDate.Date - currentDate.Date).Days >= 15 && a.AuditDate.Date > currentDate.Date).ToList();
                var stockAuditDto = filterUpcomingAudit.Select(a => new StockAuditDto
                {
                    CarId = a.CarId,
                    CarName = a.Car.CarName,
                    Variant = a.Car.Variant,
                    DaysLeftToVerify = $"{(int)(a.AuditDate - currentDate).Days} days left to verify",
                }).ToList();

                return new ResponseDto { Success = true, Data = stockAuditDto };
            }

            catch (Exception ex)
            {
                return new ResponseDto { Success = false, Message = $"Error fetching upcoming audits:{ex.Message}" };
            }
        }

        public async Task<ResponseDto> GetPendingAuditsAsync(string userId)
        {
            try
            {

                if (!int.TryParse(userId, out int userIdInt))
                {
                    return new ResponseDto { Success = false, Message = "Invalid user ID" };
                }

                var currentDate = DateTime.UtcNow;

                var userCars = await _db.Cars.Where(c => c.UserId == userIdInt).ToListAsync();
                var carIds = userCars.Select(car => car.CarId).ToList();

                var pendingAudits = await _db.StockAudits
                    .Include(a => a.Car)
                    .Where(p => carIds.Contains(p.CarId))
                    .ToListAsync();

                var filterPendingAudit = pendingAudits.Where(a => (a.AuditDate.Date - currentDate.Date).Days < 15 && a.AuditDate.Date > currentDate.Date).ToList();
                var stockAuditDto = filterPendingAudit.Select(a => new StockAuditDto
                {
                    CarId = a.CarId,
                    CarName = a.Car.CarName,
                    Variant = a.Car.Variant,
                    DaysLeftToVerify = $"{(int)(a.AuditDate - currentDate).Days} days left to verify",
                }).ToList();
                return new ResponseDto { Success = true, Data = stockAuditDto };
            }

            catch (Exception ex)
            {
                return new ResponseDto { Success = false, Message = $"Error fetching pending audits:{ex.Message}" };
            }
        }
        public async Task<ResponseDto> GetStockStatusAsync(string userId)
        {
            try
            {
               

            
                if (!int.TryParse(userId, out int userIdInt))
                {
                    return new ResponseDto { Success = false, Message = "Invalid user ID" };
                }

                var userCars = await _db.Cars.Where(c => c.UserId == userIdInt).ToListAsync();
                var carIds = userCars.Select(car => car.CarId).ToList();

                var stockAudits = await _db.StockAudits
                    .Include(p => p.Car)
                    .Where(p => carIds.Contains(p.CarId))
                    .ToListAsync();

                if (stockAudits == null)
                {
                    return new ResponseDto { Success = false, Message = "Stock status not found" };
                }

                var stockDto = stockAudits.Select(p => new StockDto
                {
                    CarName = p.Car.CarName,
                    Variant = p.Car.Variant,
                    CarId = p.Car.CarId,
                    AuditDate = p.AuditDate,
                    Status = p.Status,
                }).ToList();

                return new ResponseDto { Success = true, Data = stockDto };
            }
            catch (Exception ex)
            {
               
                return new ResponseDto { Success = false, Message = $"Error fetching stock status:{ex.Message}" };
            }
        }
        public async Task<ResponseDto> GetUserAddressesAsync(string userId)
        {
            try
            {
                int userIdInt;
                if (!int.TryParse(userId, out userIdInt))
                {
                    return new ResponseDto {Success = false, Message = "Invalid user ID" };
                }

                var userAddresses = await _db.RegisterAddresses
                    .Where(a => a.IdU == userIdInt)
                    .Select(a => new AddressDto
                    {
                        Id = a.Id,
                        Address = a.Address,
                        AddressType = a.AddressType
                    })
                    .ToListAsync();

                if (userAddresses == null)
                {
                    return new ResponseDto { Success = false, Message = "No addresses found for the user" };
                }

                return new ResponseDto { Success = true, Data = userAddresses };
            }
            catch (Exception ex)
            {
                
                return new ResponseDto { Success = false, Message = $"Error fetching user addresses:{ex.Message}" };
            }
        }
    }
}
