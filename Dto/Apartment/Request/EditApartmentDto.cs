using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Apartment.Request
{
    public class EditApartmentDto
    {
        public int heartWallArea { get; set; }
        public int clearanceArea { get; set; }
        public int bedroomAmount { get; set; }
    }
}
