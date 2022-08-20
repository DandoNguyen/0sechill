﻿using _0sechill.Models;

namespace _0sechill.Hubs.Model
{
    public class Message
    {
        #region Properties

        public Guid Id { get; set; }
        public string message { get; set; }

        #endregion

        #region Time Stamps

        public DateTime createdDateTime { get; set; }

        #endregion

        #region Author - Foreign Key

        public string userId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid roomId { get; set; }
        public Room Room { get; set; }

        #endregion
    }
}
