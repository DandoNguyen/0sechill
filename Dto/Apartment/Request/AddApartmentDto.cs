using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Apartment.Request
{
    public class AddApartmentDto
    {
        [Required]
        public int flourNo { get; set; }
        [Required]
        public int roomNo { get; set; }
        [Required]
        public string apartmentType { get; set; }
        [Required]
        public string blockId { get; set; }
    }
}
