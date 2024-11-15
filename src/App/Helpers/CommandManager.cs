using System.Text.RegularExpressions;
using App.Repository;

namespace App.Helpers;

public partial class CommandManager(HotelsRepository hotelsRepository, BookingsRepository bookingsRepository, IDateProvider dateProvider)
{
    private readonly HotelsRepository hotelsRepository = hotelsRepository;
    private readonly BookingsRepository bookingsRepository = bookingsRepository;
    private readonly IDateProvider dateProvider = dateProvider;

    public string ExecuteCommand(string command)
    {
        return command switch
        {
            var cmd when command.StartsWith(Availability) => GetAvailabilityCount(cmd),
            var cmd when command.StartsWith(Search) => SearchAvailability(cmd),
            _ => "Unknown command"
        };
    }

    private string GetAvailabilityCount(string cmd)
    {
        var regex = AvailabilityRegex();
        var match = regex.Match(cmd);
        var hotelId = match.Groups[HotelId].Value;
        var roomType = match.Groups[RoomType].Value;
        var dateRange = match.Groups[DateRange].Value.Split("-");
        var startDate = DateOnly.ParseExact(dateRange[0], Configuration.DateFormat);
        var endDate = dateRange.Length > 1 ? DateOnly.ParseExact(dateRange[1], Configuration.DateFormat) : startDate;

        var rooms = hotelsRepository.GetNumberOfRooms(hotelId, roomType);
        var booked = bookingsRepository.GetOverlappingBookings(hotelId, roomType, startDate, endDate).Count();

        var available = rooms - booked;

        return available.ToString();
    }

    private string SearchAvailability(string cmd)
    {
        var regex = SearchRegex();
        var match = regex.Match(cmd);
        var hotelId = match.Groups[HotelId].Value;
        var roomType = match.Groups[RoomType].Value;
        var days = int.Parse(match.Groups[Days].Value);
        var searchStart = dateProvider.Today();
        var searchEnd = searchStart.AddDays(days);

        var rooms = hotelsRepository.GetNumberOfRooms(hotelId, roomType);
        var available = bookingsRepository.GetAvailableRanges(hotelId, roomType, searchStart, searchEnd, rooms);

        var output = string.Join(",\n", available.Select(x =>
        {
            var start = x.Key.Item1.ToString(Configuration.DateFormat);
            var end = x.Key.Item1 != x.Key.Item2 ? "-" + x.Key.Item2.ToString(Configuration.DateFormat) : "";
            return string.Format("({0}{1}, {2})", start, end, x.Value);
        }));

        return output;
    }

    [GeneratedRegex($"{Availability}\\((?<{HotelId}>(.*?)), (?<{DateRange}>(.*?)), (?<{RoomType}>(.*?))\\)")]
    private static partial Regex AvailabilityRegex();
    [GeneratedRegex($"{Search}\\((?<{HotelId}>(.*?)), (?<{Days}>(.*?)), (?<{RoomType}>(.*?))\\)")]
    private static partial Regex SearchRegex();

    private const string Availability = "Availability";
    private const string Search = "Search";
    private const string HotelId = "hotelId";
    private const string RoomType = "roomType";
    private const string DateRange = "dateRange";
    private const string Days = "days";
}


