using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class SocialRecognization
    {
        [Key]
        public Guid Id { get; set; }
        public string number { get; set; }
        public string type { get; set; } //can only be one of the following (CMND, CCCD, CMT, PASSPORT)
    }
}
