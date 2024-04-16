using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Common
{
    public class UserParameters
    {
        public string? Username { get; set; }
        public bool? IsActive { get; set; }
        public int? RoleId { get; set; }
        public PagingFilteringParameters? PagingFilteringParameters { get; set; }
    }
}
