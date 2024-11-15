using App.Repository;

if (args.Length < 4)
{
    Console.WriteLine(@"
Please provide the path to the hotels bookings.
hotel --hotels hotels.json --bookings bookings.json
");
    return;
}

var sources = new Dictionary<string, string>();
for (var i = 0; i < args.Length; i += 2)
{
    sources.Add(args[i][2..], args[i + 1]);
}

var hotels = HotelsRepository.InitHotels(sources["hotels"]);
var bookings = BookingsRepository.InitBookings(sources["bookings"]);

while (true)
{
    var command = Console.ReadLine();
    if (command == "")
    {
        break;
    }
}

