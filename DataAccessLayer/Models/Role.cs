namespace DataAccessLayer.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleType { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
