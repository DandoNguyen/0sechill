namespace _0sechill.Hubs.Model
{
    public class Room
    {
        public Room()
        {
            isGroupChat = true;
        }
        public Guid ID { get; set; }
        public bool isGroupChat { get; set; }
        public string roomName { get; set; }
        public ICollection<Message> messages { get; set; }
    }
}
