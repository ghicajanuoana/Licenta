namespace DeviceSimulation
{
    public class Heartbeat
    {
        public int DeviceId { get; set; }

        public DateTime LastPingedTs { get; set; }
    }
}
