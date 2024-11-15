using App.Helpers;
using App.Models;

namespace App.Repository;

public class BookingsRepository
{
    private BookingsRepository(IList<Booking> bookings)
    {
        this.bookings = bookings;
    }

    public IList<Booking> Bookings => bookings;
    private readonly IList<Booking> bookings = [];

    public static BookingsRepository InitBookings(string jsonPath)
    {
        var bookings = FileReader.DeserializeJson<Booking[]>(jsonPath);

        return bookings is not null
            ? new BookingsRepository(bookings)
            : new BookingsRepository([]);
    }
}
