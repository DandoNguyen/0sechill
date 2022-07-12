namespace _0sechill.Models.Account
{
    public class Department
    {
        public Guid departmentId { get; set; }
        public string departmentName { get; set; }

        //ForeignKet offset
        public ICollection<ApplicationUser> users { get; set; }
    }
}
