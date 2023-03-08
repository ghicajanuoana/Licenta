using Common;
using DataAccessLayer.Enums;

namespace DataAccessLayer.FilterModels
{
    public class MaintenanceFilter
    {
        public string? Device { get; set; }
        public string? Description { get; set; }
        public string? Outcome { get; set; }
        public Status? Status { get; set; }
        public DateTime? ScheduledDateStart { get; set; }
        public DateTime? ScheduledDateEnd { get; set; }
        public DateTime? ActualDateStart { get; set; }
        public DateTime? ActualDateEnd { get; set; }
        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public string? CreatedBy { get; set; }
        public PagingFilteringParameters? PagingFilteringParameters { get; set; }
    }
}