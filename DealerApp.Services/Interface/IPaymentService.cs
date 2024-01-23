

using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentPayDto>> GetDuePayments(int userId);
        Task<IEnumerable<PaymentPayDto>> GetUpcomingPayments(int userId);
        Task<IEnumerable<PaymentHistoryDto>> GetPaymentStatus(int userId);
        Task<PaymentPayDto> GetPaymentDetailsWithBankDetails(int paymentId);

    }
}
