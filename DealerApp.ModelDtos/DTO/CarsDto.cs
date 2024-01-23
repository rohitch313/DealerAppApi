using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DealerApp.Dtos.DTO
{
    public class CarsDto
    {

        public int CarId { get; set; }

        public string CarName { get; set; }
        public string Variant { get; set; }
        public int UserId { get; set; }

    }
}
