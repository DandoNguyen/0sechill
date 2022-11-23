using _0sechill.Models.IssueManagement;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models.LookUpData
{
    public class LookUpTable
    {
        [Key]
        public Guid lookUpID { get; set; }
        public string lookUpTypeName { get; set; }
        public string lookUpTypeCode { get; set; }
        public string index { get; set; }
        public string valueString { get; set; }

    }
}
