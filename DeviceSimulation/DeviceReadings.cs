namespace DeviceSimulation
{
    public class DeviceReadings
    {
        public double ValueRead { get; set; }

        public string DeviceReadingTypeName { get; set; }

        public int DeviceId { get; set; }

        public DateTime ReceivedTs { get; set; }
    }
}
