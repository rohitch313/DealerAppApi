

using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;

namespace DealerAPI.Buisness_Layer.Services
{
    public class PaymentProofImgService : IPaymentProofImgService
    {
        private readonly ApplicationDbContext _db;
        

        public PaymentProofImgService(ApplicationDbContext db)
        {
            _db = db;
            
        }

        public async Task<string> UploadPaymentProofImageAsync(int paymentId, PaymentProofImgDTO proofImgDTO)
        {
            try
            {
                if (proofImgDTO == null || proofImgDTO.PaymentProofImg == null)
                {
                    return "Invalid image data";
                }

                var existingPayment = await _db.Payment.FindAsync(paymentId);

                if (existingPayment == null)
                {
                    return "Payment not found";
                }


                string uniqueIdentifier = Guid.NewGuid().ToString();

                string apiProjectDirectory = AppDomain.CurrentDomain.BaseDirectory;

                string imagePathRelative = Path.Combine("Images", $"{uniqueIdentifier}.jpg"); 
                string imagePathAbsolute = Path.Combine(apiProjectDirectory, imagePathRelative);

                // Ensure the directory exists, create it if not
                Directory.CreateDirectory(Path.GetDirectoryName(imagePathAbsolute));


                existingPayment.PaymentProofImg = imagePathRelative; 
                await _db.SaveChangesAsync();

                return "Upload successful";
            }
            catch (Exception ex)
            {

                return $"Internal Server Error: {ex.Message}";
            }
        }
    }
}