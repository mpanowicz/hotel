namespace App.Models;

public class Booking
{
    public required string HotelId { get; set; }
    public required DateOnly Arrival { get; set; }
    public required DateOnly Departure { get; set; }
    public required string RoomType { get; set; }
    public required string RoomRate { get; set; }
}
