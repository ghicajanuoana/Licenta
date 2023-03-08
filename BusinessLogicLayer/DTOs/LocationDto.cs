namespace BusinessLogicLayer.DTOs
{
    public class LocationDto
    {
        public int LocationId { get; set; }

        public string Name { get; set; }

        public bool EmailAlertsActive { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public bool IsLocationUsed { get; set; }
    }
}
