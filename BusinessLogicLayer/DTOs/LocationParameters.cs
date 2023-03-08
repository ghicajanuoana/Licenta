using Common;

namespace BusinessLogicLayer.DTOs
{
    public class LocationParameters
    {      
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? ContactEmail { get; set; }
        public PagingFilteringParameters? PagingFilteringParameters { get; set; }
    }
}