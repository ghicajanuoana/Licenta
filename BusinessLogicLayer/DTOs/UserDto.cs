using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and Confirmation Password do not match")]
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }

        //public string RoleType{ get; set; }
    }
}