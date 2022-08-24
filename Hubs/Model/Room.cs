using _0sechill.Models;

namespace _0sechill.Hubs.Model
{
    public class Room
    {
        public Room()
        {
            ID = Guid.NewGuid();
            isGroupChat = true;
        }
        public Guid ID { get; set; }
        public bool isGroupChat { get; set; }
        public string roomName { get; set; }
        public string groupAdmin { get; set; }
        public ICollection<ApplicationUser> users { get; set; }
        public ICollection<Message> messages { get; set; }
    }
}
