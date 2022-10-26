namespace _0sechill.Hubs.Model
{
    public class Notifications
    {
        public Notifications()
        {
            ID = Guid.NewGuid();
            isSeen = false;
        }
        public Guid ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string receiverId { get; set; }
        public bool isSeen { get; set; }
    }
}
