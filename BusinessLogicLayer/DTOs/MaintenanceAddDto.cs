
namespace BusinessLogicLayer.DTOs
{
    public class MaintenanceAddDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }

        public DeviceDto Device { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string? Description { get; set; }
    }
}
