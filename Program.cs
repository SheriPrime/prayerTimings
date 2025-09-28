using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using HttpClient client = new HttpClient();

        string geoUrl = "http://ip-api.com/json/";
        string geoJson = await client.GetStringAsync(geoUrl);
        var geoDoc = JsonDocument.Parse(geoJson).RootElement;

        // Console.WriteLine(geoDoc);

        double lat = geoDoc.GetProperty("lat").GetDouble();
        double lon = geoDoc.GetProperty("lon").GetDouble();

        Console.WriteLine($"Detected Location: {geoDoc.GetProperty("city")}, {geoDoc.GetProperty("country")}");
        Console.WriteLine($"Latitude: {lat}, Longitude: {lon}");

        string prayerUrl = $"https://api.aladhan.com/v1/timings?latitude={lat}&longitude={lon}&method=2";
        string prayerJson = await client.GetStringAsync(prayerUrl);

        var prayerDoc = JsonDocument.Parse(prayerJson).RootElement;
        var timings = prayerDoc.GetProperty("data").GetProperty("timings");

        Console.WriteLine("\nPrayer Times:");
        Console.WriteLine("Fajr: " + timings.GetProperty("Fajr").GetString());
        Console.WriteLine("Dhuhr: " + timings.GetProperty("Dhuhr").GetString());
        Console.WriteLine("Asr: " + timings.GetProperty("Asr").GetString());
        Console.WriteLine("Maghrib: " + timings.GetProperty("Maghrib").GetString());
        Console.WriteLine("Isha: " + timings.GetProperty("Isha").GetString());
    }
}
