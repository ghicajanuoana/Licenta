using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.DTOs
{
    public class DeviceDtoAdd : DeviceDto
    {
        public string? SoftwareVersion { get; set; }

        public string? FirmwareVersion { get; set; }

        public string? Alias { get; set; }

        public string? Emails { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
