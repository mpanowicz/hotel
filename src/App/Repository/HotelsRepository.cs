using App.Models;
using App.Helpers;

namespace App.Repository;

public class HotelsRepository
{
    private HotelsRepository(IList<Hotel> hotels)
    {
        this.hotels = hotels;
    }

    public IEnumerable<Hotel> Hotels => hotels;
    private readonly IEnumerable<Hotel> hotels = [];

    public static HotelsRepository InitHotels(string jsonPath)
    {
        var hotels = FileReader.DeserializeJson<Hotel[]>(jsonPath);

        return hotels is not null
            ? new HotelsRepository(hotels)
            : new HotelsRepository([]);
    }

    public int GetNumberOfRooms(string hotelId, string roomType)
    {
        return Hotels
            .Where(x => x.Id == hotelId)
            .SelectMany(x => x.Rooms)
            .Where(x => x.RoomType == roomType)
            .Count();
    }
}

