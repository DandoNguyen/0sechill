using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class Apartment
    {
        [Key]
        public Guid apartmentId { get; set; }
        public int heartWallArea { get; set; }
        public int clearanceArea { get; set; }
        public bool ísFurnitureAvailable { get; set; }
        public int bedroomAmount { get; set; }

        //FK
        public Block block { get; set; }
        [Required]
        public Guid blockId { get; set; }
        public ICollection<UserHistory> userHistories { get; set; }
    }
}
