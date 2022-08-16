using _0sechill.Models;

namespace _0sechill.Hubs.Model
{
    public class UserConnection
    {
        #region Props

        public Guid ID { get; set; }

        #endregion

        #region Foreign Key

        public string roomId { get; set; }
        public virtual Room Room { get; set; }

        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }

        #endregion
    }
}
