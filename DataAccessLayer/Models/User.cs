namespace DataAccessLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public bool? IsActive { get; set; }

        public int? RoleId { get; set; }

        //public string? RoleType { get; set; }

        public Role? Role { get; set; }

        public string? Token { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}