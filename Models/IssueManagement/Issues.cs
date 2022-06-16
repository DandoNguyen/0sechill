using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models.IssueManagement
{
    public class Issues
    {
        [Key]
        public Guid ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string status { get; set; }

        //TimeStamps
        public DateOnly createdDate { get; set; }
        public DateOnly lastModifiedDate { get; set; }
        [Required]
        public bool isPrivate { get; set; }

        //FK
        [Required]
        public Guid cateId { get; set; }
        public Category category { get; set; }
        public string authorId { get; set; }
        public ApplicationUser author { get; set; }

        //Collection offset
        public ICollection<FilePath> files { get; set; }
        public ICollection<Comments> comments { get; set; }

        //On new Object event
        public Issues()
        {
            ID = Guid.NewGuid();
            createdDate = DateOnly.FromDateTime(new DateTime());
            lastModifiedDate = DateOnly.FromDateTime(new DateTime());
        }
    }
}
