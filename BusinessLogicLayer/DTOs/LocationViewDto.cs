namespace BusinessLogicLayer.DTOs
{
    public class LocationViewDto
    {
        public int LocationId { get; set; }

        public string Name { get; set; }

        public string? ContactEmail { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public bool IsLocationUsed { get; set; }
    }
}
