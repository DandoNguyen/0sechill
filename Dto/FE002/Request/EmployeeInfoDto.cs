using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.FE002.Request
{
    public class EmployeeInfoDto
    {
        public string UserName { get; set; }
        public string fullname { get; set; }
        public bool isMale { get; set; }
        public string nationality { get; set; }
        public string country { get; set; }
        public DateTime DOB { get; set; }
        public int age { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string roleID { get; set; }
        public string residentialAddress { get; set; }
        public string Email { get; set; }
        public string phoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string apartmentID { get; set; }
        [Required]
        public bool isStaff { get; set; }

    }
}
