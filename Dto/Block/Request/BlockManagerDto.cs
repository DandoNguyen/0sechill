using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Block.Request
{
    public class BlockManagerDto
    {
        [Required]
        public string blockId { get; set; }
        [Required]
        public string userId { get; set; }
    }
}
