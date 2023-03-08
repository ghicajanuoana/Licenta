namespace BusinessLogicLayer.DTOs
{
    public class DevicePingDto
    {
        public int DeviceId { get; set; }
        public DateTime LastPingedTs { get; set; }
    }
}