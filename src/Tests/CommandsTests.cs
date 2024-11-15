using App.Helpers;
using App.Repository;

namespace Tests;

public class CommandsTests
{
    private readonly CommandManager commandManager;

    public CommandsTests()
    {
        var hotelsRepository = HotelsRepository.InitHotels("hotels.json");
        var bookingsRepository = BookingsRepository.InitBookings("bookings.json");
        commandManager = new CommandManager(hotelsRepository, bookingsRepository, new FakeDateProvider());
    }

    [Theory]
    [InlineData("Availability(H1, 20240901, SGL)", 2)]
    [InlineData("Availability(H1, 20240901-20240903, DBL)", 1)]
    public void GetAvailabilityCount(string command, int count)
    {
        var result = commandManager.ExecuteCommand(command);
        Assert.Equal(count.ToString(), result);
    }

    [Theory]
    [InlineData("Search(H1, 365, SGL)", "(20240901, 2),\n(20240902-20240905, 1),\n(20240906-20250901, 2)")]
    [InlineData("Search(H1, 1, DBL)", "(20240901-20240902, 1)")]
    public void SearchAvailableRanges(string cmd, string expected)
    {
        var result = commandManager.ExecuteCommand(cmd);

        Assert.Equal(expected, result);
    }
}

internal class FakeDateProvider : IDateProvider
{
    public DateOnly Today()
    {
        return new DateOnly(2024, 9, 1);
    }
}
