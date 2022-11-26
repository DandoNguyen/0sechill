using _0sechill.Models.LookUpData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _0sechill.Models.IssueManagement
{
    public class Issues
    {
        [Key]
        public Guid ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public string feedback { get; set; }
        public int priorityLevel { get; set; }

        //TimeStamps
        public DateTime createdDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        [Required]
        public bool isPrivate { get; set; }

        //FK
        [Required]
        public virtual ApplicationUser author { get; set; }

        public virtual AssignIssue assignIssue { get; set; }

        //Collection offset
        public virtual ICollection<FilePath> files { get; set; }
        public virtual ICollection<Comments> comments { get; set; }
        public virtual ICollection<LookUpTable> listCateLookUp { get; set; }
        public virtual ICollection<Vote> votes { get; set; }
        public virtual LookUpTable statusLookUp { get; set; }

        //On new Object event
        public Issues()
        {
            ID = Guid.NewGuid();
            createdDate = DateTime.Today;
            lastModifiedDate = DateTime.Today;
            feedback = string.Empty;
            priorityLevel = 0;
        }
    }
}
