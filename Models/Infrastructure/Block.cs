using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _0sechill.Models
{
    public class Block
    {
        [Key]
        public Guid blockId { get; set; }
        [Required]
        public string blockName { get; set; }
        public int flourAmount { get; set; }

        //Block Manager Offset foreign key
        public string blockManagerId { get; set; }
        [Column("BlockManagerRef")]
        public ApplicationUser blockManager { get; set; }

        public ICollection<Apartment> apartments { get; set; }
    }
}
