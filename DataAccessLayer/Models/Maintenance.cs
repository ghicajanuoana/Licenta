using DataAccessLayer.Enums;

namespace DataAccessLayer.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public Device Device { get; set; }
        public int DeviceId { get; set; }
        public string? Description { get; set; }
        public string? Outcome { get; set; }
        public Status Status { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
