using _0sechill.Models;

namespace _0sechill.Hubs.Model
{
    public class UserRoom
    {
        public string userID { get; set; }
        public ApplicationUser Users { get; set; }
        public Guid roomID { get; set; }
        public Room rooms { get; set; }
    }
}
