namespace DataAccessLayer.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        public string Name { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public bool EmailAlertsActive { get; set; }

        public string? EmailRecipient { get; set; }

        //public int UserId { get; set; }

        //public User User { get; set; }

        public string? ContactEmail { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string? Phone { get; set; }
    }
}
