using System.ComponentModel.DataAnnotations;

namespace _0sechill.Hubs.Dto
{
    public class AddNewRoomDto
    {
        [Required]
        public string roomName { get; set; }
        [Required]
        public List<string> listUserId { get; set; }
    }
}
