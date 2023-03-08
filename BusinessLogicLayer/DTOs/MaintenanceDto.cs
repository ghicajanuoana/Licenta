using DataAccessLayer.Enums;

namespace BusinessLogicLayer.DTOs
{
    public class MaintenanceDto
    {
        public int Id { get; set; }
        public MaintenanceDeviceDto MaintenanceDeviceDto { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? Description { get; set; }
        public string? Outcome { get; set; }
    }
}
