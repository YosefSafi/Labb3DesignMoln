namespace Labb3TravelBookingAPI.Models
{
    public class Booking
    {
        public string Id { get; set; }
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerName { get; set; }
    }
}
