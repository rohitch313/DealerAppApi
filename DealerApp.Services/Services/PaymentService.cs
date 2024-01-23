
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace DealerAPI.Buisness_Layer.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _db;

        public PaymentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PaymentPayDto>> GetDuePayments(int userId)
        {
            try
            {
                var currentDate = DateTime.UtcNow;

                var userCars = await _db.Cars.Where(c => c.UserId == userId).ToListAsync();
                var carIds = userCars.Select(car => car.CarId).ToList();
                var duePayments = await _db.Payment
    .Include(p => p.Car)
    .Include(p => p.BankDetail)
    .Where(p => carIds.Contains(p.CarId))
    .ToListAsync();
                var filteredDuePayments = duePayments
                                      .Where(p => DateTime.Compare(currentDate.Date, p.DueDate.Date) < 0 &&
                                                  (p.DueDate.Date - currentDate.Date).Days <= 60)
                                      .ToList(); //Retrieve filtered payments from the database

                var paymentDtos = filteredDuePayments.Select(p => new PaymentPayDto
                {
                    Id = p.Id,
                    Amount_Due = p.Amount_Due,
                    CarId = p.CarId,
                    CarName = p.CarName,
                    Variant = p.Variant,
                    DueDate = p.DueDate,
                    DaysLeft = $"{(p.DueDate - currentDate).Days}/60",
                    StartDate = p.StartDate,
                    AmountPaid = p.AmountPaid,
                    ProcessingCharges = p.ProcessingCharges,
                    Name = p.Name,
                    AccountNumber = p.AccountNumber,
                    BankName = p.BankName,
                    IFSCCode = p.IFSCCode,
                    UserId = p.Userid
                }).ToList();

                return paymentDtos;

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<PaymentPayDto>> GetUpcomingPayments(int userId)
        {
            try
            {
                var currentDate = DateTime.UtcNow;

                var userCars = await _db.Cars.Where(c => c.UserId == userId).ToListAsync();
                var carIds = userCars.Select(car => car.CarId).ToList();
                var upcomingPayments = await _db.Payment
                    .Include(p => p.Car)
                    .Include(p => p.BankDetail)
                    .Where(p => carIds.Contains(p.CarId))
                    .ToListAsync();

                var filteredUpcomingPayments = upcomingPayments
                    .Where(p => DateTime.Compare(currentDate.Date, p.DueDate.Date) < 0 &&
                                (p.DueDate.Date - currentDate.Date).Days > 60)
                    .ToList();

                var paymentDtos = filteredUpcomingPayments.Select(p => new PaymentPayDto
                {
                    Id = p.Id,
                    Amount_Due = p.Amount_Due,
                    CarId = p.CarId,
                    CarName = p.CarName,
                    Variant = p.Variant,
                    DueDate = p.DueDate,
                    DaysLeft = $"{(p.DueDate - currentDate).Days}/60",
                    StartDate = p.StartDate,
                    AmountPaid = p.AmountPaid,
                    ProcessingCharges = p.ProcessingCharges,
                    Name = p.Name,
                    AccountNumber = p.AccountNumber,
                    BankName = p.BankName,
                    IFSCCode = p.IFSCCode
                }).ToList();

                return paymentDtos;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<PaymentHistoryDto>> GetPaymentStatus(int userId)
        {
            try
            {
                var currentDate = DateTime.UtcNow;

                var userCars = await _db.Cars.Where(c => c.UserId == userId).ToListAsync();
                var carIds = userCars.Select(car => car.CarId).ToList();

                var paymentStatusList = await _db.Payment
                .Where(p => p.PaymentStatus != null)
                   .Where(p => carIds.Contains(p.CarId))
                .Include(p => p.Car)
                .ToListAsync();

                var paymentHistoryDtos = paymentStatusList.Select(p => new PaymentHistoryDto
                {
                    Id = p.Id,
                    Amount_Due = p.Amount_Due,
                    CarId = p.CarId,
                    CarName = p.CarName,
                    Variant = p.Variant,
                    PaymentStatus = (PaymentDto.paymentStatus?)p.PaymentStatus
                }).ToList();

                return paymentHistoryDtos;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<PaymentPayDto> GetPaymentDetailsWithBankDetails(int paymentId)
        {
            try
            {
                var currentDate = DateTime.UtcNow;

                var payment = await _db.Payment
                    .Include(p => p.Car)
                    .Include(p => p.BankDetail)
                    .FirstOrDefaultAsync(p => p.Id == paymentId);

                if (payment == null)
                {
                    // Handle the case where the payment is not found
                    return null;
                }

                var paymentDto = new PaymentPayDto
                {
                    Id = paymentId,
                    Amount_Due = payment.Amount_Due,
                    CarId = payment.CarId,
                    CarName = payment.CarName,
                    Variant = payment.Variant,
                    DaysLeft = $"{(payment.DueDate - currentDate).Days}/60",
                    DueDate = payment.DueDate,
                    StartDate = payment.StartDate,
                    AmountPaid = payment.AmountPaid,
                    ProcessingCharges = payment.ProcessingCharges,
                    Name = payment.Name,
                    AccountNumber = payment.AccountNumber,
                    BankName = payment.BankName,
                    IFSCCode = payment.IFSCCode
                };

                return paymentDto;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }
        }

}