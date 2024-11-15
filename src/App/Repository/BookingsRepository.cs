using App.Helpers;
using App.Models;

namespace App.Repository;

public class BookingsRepository
{
    private BookingsRepository(IList<Booking> bookings)
    {
        this.bookings = bookings;
    }

    public IEnumerable<Booking> Bookings => bookings;
    private readonly IEnumerable<Booking> bookings = [];

    public static BookingsRepository InitBookings(string jsonPath)
    {
        var bookings = FileReader.DeserializeJson<Booking[]>(jsonPath);

        return bookings is not null
            ? new BookingsRepository(bookings)
            : new BookingsRepository([]);
    }

    public IEnumerable<Booking> GetOverlappingBookings(string hotelId, string roomType, DateOnly searchStart, DateOnly searchEnd)
    {
        var bookings = Bookings
            .Where(x => x.HotelId == hotelId)
            .Where(x => x.RoomType == roomType)
            .Where(x => !(searchStart > x.Departure || searchEnd < x.Arrival));
        return bookings;
    }

    public IDictionary<(DateOnly, DateOnly), int> GetAvailableRanges(string hotelId, string roomType, DateOnly searchStart, DateOnly searchEnd, int rooms)
    {
        var booked = GetOverlappingBookings(hotelId, roomType, searchStart, searchEnd);

        var bookedCount = new Dictionary<DateOnly, int>();
        foreach (var b in booked)
        {
            for (var date = b.Arrival; date <= b.Departure; date = date.AddDays(1))
            {
                bookedCount.TryGetValue(date, out var count);
                bookedCount[date] = count + 1;
            }
        }

        var ranges = new Dictionary<(DateOnly, DateOnly), int>();
        for (var date = searchStart; date <= searchEnd; date = date.AddDays(1))
        {
            var rangeStart = date;
            if (bookedCount.TryGetValue(rangeStart, out var count))
            {
                while (bookedCount.TryGetValue(date, out var next) && count == next && date <= searchEnd)
                {
                    date = date.AddDays(1);
                }
            }
            else
            {
                while (!bookedCount.ContainsKey(date) && date <= searchEnd)
                {
                    date = date.AddDays(1);
                }
            }
            date = date.AddDays(-1);

            if (count < rooms)
            {
                ranges.Add((rangeStart, date), rooms - count);
            }
        }

        return ranges;
    }
}
