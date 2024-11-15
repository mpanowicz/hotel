using App.Helpers;
using App.Repository;

if (args.Length < 4)
{
    Console.WriteLine(@"
Please provide the path to the hotels bookings.
App.exe --hotels hotels.json --bookings bookings.json
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
var commandManager = new CommandManager(hotels, bookings, new DateProvider());

while (true)
{
    var command = Console.ReadLine();
    if (string.IsNullOrEmpty(command))
    {
        break;
    }
    else
    {
        Console.WriteLine(commandManager.ExecuteCommand(command));
    }
}

