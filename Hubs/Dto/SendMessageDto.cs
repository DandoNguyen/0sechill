using System.ComponentModel.DataAnnotations;

namespace _0sechill.Hubs.Dto
{
    public class SendMessageDto
    {
        [Required]
        public string message { get; set; }
        [Required]
        public string userId { get; set; }
        public string userConnectionId { get; set; }
        public string receiverId { get; set; }
    }
}
