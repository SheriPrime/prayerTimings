using System.Text.Json;
using IPGeolocation;
using Pastel;
using System.Drawing;

class Program
{
    enum Salah
    {
        fajr,
        dhuhr,
        asr,
        maghrib,
        isha
    }

    enum Sect
    {
        Shafi,
        Hanafi,
        Maliki,
        Hanbali
    }

    static String selectSect()
    {
        Console.WriteLine("Select your Islamic sect:");
        Console.WriteLine("1. Shafi");
        Console.WriteLine("2. Hanafi");
        Console.WriteLine("3. Maliki");
        Console.WriteLine("4. Hanbali");
        Console.Write("Enter the number corresponding to your sect (Default is Hanafi): ");
        string choice = Console.ReadLine() ?? string.Empty;
        return choice switch
        {
            "1" => Sect.Shafi.ToString(),
            "2" => Sect.Hanafi.ToString(),
            "3" => Sect.Maliki.ToString(),
            "4" => Sect.Hanbali.ToString(),
            _ => Sect.Hanafi.ToString(),
        };
    }
    
    static string offsetTime(string time, double offset)
    {
        TimeSpan ts = TimeSpan.Parse(time);
        ts = ts.Add(TimeSpan.FromHours(offset));
        return ts.ToString(@"hh\:mm");
    }

    static async Task Main(string[] args)
    {
        using HttpClient client = new HttpClient();

        string apiKey = "8dd225fd884548768ba7a93419947505";

        IPGeolocationAPI api = new IPGeolocationAPI(apiKey);
        GeolocationParams geoParams = new GeolocationParams();
        Geolocation location = api.GetGeolocation(geoParams);

        Timezone tz = api.GetTimezone();
        double offset = tz.GetTimezoneOffset();

        string lat = double.Parse(location.GetLatitude()).ToString("0.0000");
        string lon = double.Parse(location.GetLongitude()).ToString("0.0000");

        string sect = selectSect();
        string prayerUrl = $"https://www.ummahapi.com/api/prayer-times?lat={lat}&lng={lon}&madhab={sect}&method=MuslimWorldLeague";

        string prayerJson = await client.GetStringAsync(prayerUrl);
        JsonElement prayerDoc = JsonDocument.Parse(prayerJson).RootElement;
        JsonElement timings = prayerDoc.GetProperty("data").GetProperty("prayer_times");

        Console.WriteLine($"\nDetected Location: {location.GetCity()}, {location.GetCountryName()}");
        Console.WriteLine($"Latitude: {lat}, Longitude: {lon}");
        Console.WriteLine("\nPrayer Times:");

        foreach (Salah salah in Enum.GetValues<Salah>())
        {
            string prayerName = salah.ToString();
            string adjustedTime = offsetTime(timings.GetProperty(prayerName).ToString(), offset);
            Console.WriteLine($"{prayerName}: {adjustedTime}");
        }
        Console.WriteLine("\nNote: " + prayerDoc.GetProperty("data").GetProperty("islamic_info").GetProperty("note").GetString());

        Console.WriteLine("\nPress any key to exit...".Pastel(Color.Red));
        Console.ReadKey();
    }
}
