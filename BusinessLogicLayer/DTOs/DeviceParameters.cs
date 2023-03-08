using Common;

namespace BusinessLogicLayer.DTOs
{
    public class DeviceParameters
    {
        public string? SerialNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? DeviceType { get; set; }
        public PagingFilteringParameters? PagingFilteringParameters { get; set; }
    }
}
