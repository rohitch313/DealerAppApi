using System.ComponentModel.DataAnnotations.Schema;

namespace DealerApp.Dtos.DTO
{
    public class Order_StockAuditDTO
    {
        public string Location { get; set; }

        public int StockAudit_PurposeId { get; set; }

        public DateTime ChooseDate { get; set; }

    }
}
