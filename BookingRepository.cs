using Labb3TravelBookingAPI.Models;

namespace Labb3TravelBookingAPI.Data
{
    public static class BookingRepository
    {
        private static readonly List<Booking> bookings = new();

        public static List<Booking> GetAll() => bookings;

        public static Booking? GetById(string id) =>
            bookings.FirstOrDefault(b => b.Id == id);

        public static void Add(Booking booking) => bookings.Add(booking);

        public static void Update(Booking updated)
        {
            var existing = GetById(updated.Id);
            if (existing is not null)
            {
                existing.Destination = updated.Destination;
                existing.StartDate = updated.StartDate;
                existing.EndDate = updated.EndDate;
                existing.CustomerName = updated.CustomerName;
            }
        }

        public static void Delete(string id)
        {
            var existing = GetById(id);
            if (existing is not null)
            {
                bookings.Remove(existing);
            }
        }
    }
}
