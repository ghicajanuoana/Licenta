using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class LocationAddDto : LocationDto
    {
        public string? EmailRecipient { get; set; }
        //public UserDto? User { get; set; }

        public string? ContactEmail { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string? Phone { get; set; }
    }
}
