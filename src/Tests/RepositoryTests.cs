using App.Repository;

namespace Tests;

public class HotelsRepositoryTests
{
    [Fact]
    public void InitHotels_ReturnsRepository()
    {
        var repository = HotelsRepository.InitHotels("hotels.json");

        Assert.Single(repository.Hotels);
    }

    [Fact]
    public void InitBookings_ReturnsRepository()
    {
        var repository = BookingsRepository.InitBookings("bookings.json");

        Assert.Equal(2, repository.Bookings.Count);
    }
}
