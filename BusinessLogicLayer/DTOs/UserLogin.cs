using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class UserLogin
    {
        public string Username { get; set; }

        public string Password { get; set; }

        //public string? RoleType { get; set; }

        public string? Token { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
