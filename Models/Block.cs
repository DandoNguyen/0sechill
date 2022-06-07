using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class Block
    {
        [Key]
        public Guid blockId { get; set; }
        [Required]
        public string blockName { get; set; }
        public int flourAmount { get; set; }
        public ICollection<Apartment> apartments { get; set; }
    }
}
