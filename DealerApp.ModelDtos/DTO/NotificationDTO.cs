using System.ComponentModel.DataAnnotations;

namespace DealerApp.Dtos.DTO
{
    public class NotificationDTO
    {
        [Required]
        [MinLength(5)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        public string Discription { get; set; }

        public DateTime MessageTime { get; set; }

        public int UserInfoId { get; set; }

    }
}
