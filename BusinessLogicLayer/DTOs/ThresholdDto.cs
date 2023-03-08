namespace BusinessLogicLayer.DTOs
{
    public class ThresholdDto
    {
        public int Id { get; set; }
        public int? DeviceTypeId { get; set; }
        public int? DeviceReadingTypeId { get; set; }
        public decimal MinValue { get; set; }
        public decimal WarningValue { get; set; }
        public decimal CriticalValue { get; set; }
        public decimal MaxValue { get; set; }
        public DeviceTypeDto? DeviceType { get; set; }
        public DeviceReadingTypeDto? DeviceReadingType { get; set; }
    }
}