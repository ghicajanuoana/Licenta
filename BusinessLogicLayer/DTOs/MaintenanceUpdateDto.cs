using DataAccessLayer.Enums;
using System;

namespace BusinessLogicLayer.DTOs
{
    public class MaintenanceUpdateDto : MaintenanceAddDto
    {
        public string? Outcome { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; }
        public string? CreatedBy { get; }
        public DateTime? ActualDate { get; set; }
    }
}
