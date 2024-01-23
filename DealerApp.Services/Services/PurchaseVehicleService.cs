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
    public class PurchaseVehicleService:IPurchaseVehicleService
    {
        private readonly ApplicationDbContext _db;
        

        public PurchaseVehicleService(ApplicationDbContext db )
        {
            _db = db;

        }
        public async Task<ResponseDto> GetVehicleRecordStatusAsync(int id)
        {
            try
            {
                
                var vehicleRecord = await _db.VehicleRecords
                    .Include(vr => vr.Car)
                    .FirstOrDefaultAsync(vr => vr.Id == id);

                if (vehicleRecord == null)
                {
                    return new ResponseDto
                    {
                        Success = false,
                        Message = "Vehicle record not found"
                    };
                }

                var failedStatus = DetermineFailedStatus(vehicleRecord);

                var vehicleRecordStatusDto = new VehicleRecordsDto
                {
                    Challan = vehicleRecord.Challan,
                    RcStatus = vehicleRecord.RcStatus,
                    Fitness = vehicleRecord.Fitness,
                    OwnerName = vehicleRecord.OwnerName,
                    Hypothecation = vehicleRecord.Hypothecation,
                    Blacklist = vehicleRecord.Blacklist,
                    CarName = vehicleRecord.Car.CarName,
                    PurchaseId = vehicleRecord.CId,
                    Variant = vehicleRecord.Car.Variant,
                   
                };

                return new ResponseDto
                {
                    Success = true,
                    Data = vehicleRecordStatusDto
                };
            }
            catch (Exception )
            {
              
                return new ResponseDto
                {
                    Success = false,
                    Message = "Internal Server Error"
                };
            }
        }

        private static string DetermineFailedStatus(Vehiclerecord vehicleRecord)
        {
            if (!vehicleRecord.Challan)
            {
                return "Challan";
            }
            else if (!vehicleRecord.RcStatus)
            {
                return "RC Status";
            }
            else if (!vehicleRecord.Fitness)
            {
                return "Fitness";
            }
            else if (!vehicleRecord.Fitness)
            {
                return "OwnerName";
            }
            else if (!vehicleRecord.Fitness)
            {
                return "Hypothecation";
            }
            else if (!vehicleRecord.Fitness)
            {
                return "Blacklist";
            }

            // Add more conditions for other status if needed

            return null; // Return null if no status has failed
        }
        public async Task<ResponseDto> GetCarsWithStatusAsync(string userIdString)
        {
            try
            {
                if (int.TryParse(userIdString, out int userId))
                {
                    var currentDate = DateTime.UtcNow;

                    var userCars = await _db.Cars.Where(c => c.UserId == userId).ToListAsync();
                    var carIds = userCars.Select(car => car.CarId).ToList();

                    var carsWithStatus = await _db.Cars
                        .Where(c => c.vehiclerecords.Any()) // Filter out cars without vehicle records
                        .Where(c => carIds.Contains(c.CarId))
                        .Include(c => c.vehiclerecords)
                        .Select(c => new CarStatusDto
                        {
                            CarName = c.CarName,
                            Variant = c.Variant,
                            PurchaseId = c.CarId,
                            ActionRequired = DetermineActionRequired(c.vehiclerecords)
                        })
                        .ToListAsync();

                    return new ResponseDto
                    {
                        Success = true,
                        Data = carsWithStatus
                    };
                }
                else
                {
                    return new ResponseDto
                    {
                        Success = false,
                        Message = "Invalid user ID"
                    };
                }
            }
            catch (Exception ex)
            {

                return new ResponseDto
                {
                    Success = false,
                    Message = $"Internal Server Error:{ex}"
                };
            }
        }

        private static string DetermineActionRequired(IEnumerable<Vehiclerecord> vehicleRecords)
        {
            if (vehicleRecords != null && vehicleRecords.Any())
            {
                foreach (var record in vehicleRecords)
                {
                    if (!record.Challan || !record.RcStatus || !record.Fitness ||
                        !record.OwnerName || !record.Hypothecation || !record.Blacklist)
                    {
                        return "Action Required"; // Return if any record indicates action is required
                    }
                }
            }

            return "No Action Required"; // Return if no action is required for any record
        }


    }
}



