using DataAccessLayer.Models;

namespace BusinessLogicLayer.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }
    }
}