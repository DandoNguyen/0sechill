using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.FE001.Request
{
    public class SearchFilterDto
    {
        public string fullName { get; set; }
        public string IDNumber { get; set; }
        public string phoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
