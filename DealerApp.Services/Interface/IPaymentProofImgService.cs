
using DealerApp.Dtos.DTO;

namespace DealerApp.Service.Interface
{
    public interface IPaymentProofImgService
    {
        Task<string> UploadPaymentProofImageAsync(int paymentId, PaymentProofImgDTO proofImgDTO);
    }
}
