using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        public string SerialNumber { get; set; }

        public string Name { get; set; }

        public string? SoftwareVersion { get; set; }

        public string? FirmwareVersion { get; set; }

        public DateTime? LastUploadTime { get; set; }

        public string? Description { get; set; }

        public string? Alias { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }

        public int DeviceTypeId { get; set; }

        public DeviceType DeviceType { get; set; }

        public byte[]? ImageBytes { get; set; }

        public string? Emails { get; set; }

        public DateTime? LastPingedTs { get; set; }
    }
}