using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Contract.Response
{
    public class ContractDto
    {
        public string ID { get; set; }
        public DateTime startDate { get; set; } = DateTime.MinValue;
        public DateTime endDate { get; set; } = DateTime.MinValue;
        public DateTime lastSignedDate { get; set; } = DateTime.MinValue;

        public DateTimeOffset createdDate { get; set; } = new DateTimeOffset();
        public DateTimeOffset modifiedDate { get; set; } = new DateTimeOffset();
        public string ownerID { get; set; }
        public string UserName { get; set; }
        public List<string> listApartmentID { get; set; } = new List<string>();
    }
}
