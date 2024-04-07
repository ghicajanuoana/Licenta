namespace DataAccessLayer.Models
{
    public class DeviceType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public string Unit { get; set; }

        public ICollection<Threshold> Thresholds { get; set; }
    }
}