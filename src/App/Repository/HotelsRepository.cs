using App.Models;
using App.Helpers;

namespace App.Repository;

public class HotelsRepository
{
    private HotelsRepository(IList<Hotel> hotels)
    {
        this.hotels = hotels;
    }

    public IList<Hotel> Hotels => hotels;
    private readonly IList<Hotel> hotels = [];

    public static HotelsRepository InitHotels(string jsonPath)
    {
        var hotels = FileReader.DeserializeJson<Hotel[]>(jsonPath);

        return hotels is not null
            ? new HotelsRepository(hotels)
            : new HotelsRepository([]);
    }
}

