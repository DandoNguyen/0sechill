using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class Apartment
    {
        [Key]
        public Guid apartmentId { get; set; }
        public string apartmentName { get; set; }
        public int heartWallArea { get; set; }
        public int clearanceArea { get; set; }
        public int bedroomAmount { get; set; }

        //FK
        public Block block { get; set; }
        [Required]
        public Guid blockId { get; set; }
        public UserHistory userHistories { get; set; }
    }
}
