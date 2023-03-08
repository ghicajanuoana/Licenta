namespace DataAccessLayer.Models
{
    public class Threshold
    {
        public int Id { get; set; }

        public int DeviceTypeId { get; set; }

        public DeviceType DeviceType { get; set; }

        public int DeviceReadingTypeId { get; set; }

        public DeviceReadingType DeviceReadingType { get; set; }

        public decimal MinValue { get; set; }

        public decimal WarningValue { get; set; }

        public decimal CriticalValue { get; set; }

        public decimal MaxValue { get; set; }
    }
}