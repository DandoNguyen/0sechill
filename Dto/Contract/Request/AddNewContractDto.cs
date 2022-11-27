using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Contract.Request
{
    public class AddNewContractDto
    {
        public List<string> listApartmentID { get; set; }
        public string residentID { get; set; }
        public DateOnly startDate { get; set; }
        [Required]
        public DateOnly endDate { get; set; }
        [Required]
        public DateOnly lastSignedDate { get; set; }
    }
}
