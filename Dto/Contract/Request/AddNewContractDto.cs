using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Contract.Request
{
    public class AddNewContractDto
    {
        public List<string> listApartmentID { get; set; }
        public string residentID { get; set; }
        public DateTime startDate { get; set; }
        [Required]
        public DateTime endDate { get; set; }
        [Required]
        public DateTime lastSignedDate { get; set; }
    }
}
