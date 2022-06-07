using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.UserHistory.Request
{
    public class AddUserHistoryDto
    {
        [Required]
        public string blockId { get; set; }
        [Required]
        public string apartmentId { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public DateOnly startDate { get; set; }
        [Required]
        public DateOnly endDate { get; set; }
        [Required]
        public string status { get; set; }
    }
}
