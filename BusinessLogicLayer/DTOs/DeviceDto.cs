using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.DTOs
{
    public class DeviceDto
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string? Description { get; set; }
        public LocationDto Location { get; set; }
        public DeviceTypeDto DeviceType { get; set; }
        public string? ImageBytes { get; set; }
    }
}
