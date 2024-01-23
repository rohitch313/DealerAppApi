using System.Numerics;

namespace DealerApp.Dtos.DTO
{
    public class ProcurementColsedDto : ProcDetailDto
    {
        public decimal? Amount_paid { get; set; }
        public DateTime? ColsedOn { get; set; }
    }
}
