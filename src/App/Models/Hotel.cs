namespace App.Models;

public class Hotel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public IList<RoomType> RoomTypes { get; set; } = [];
    public IList<Room> Rooms { get; set; } = [];
}

public class RoomType
{
    public required string Code { get; set; }
    public required string Description { get; set; }
    public string[] Amenities { get; set; } = [];
    public string[] Features { get; set; } = [];
}

public class Room
{
    public required string RoomId { get; set; }
    public required string RoomType { get; set; }
}