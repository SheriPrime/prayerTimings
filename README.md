# PrayerTimings

A simple C# app to fetch **Namaz / Prayer Times** for a given location using APIs.

## ⚙️ Features

* Detects user location (via IP geolocation or manual lat/long)
* Supports multiple calculation methods / madhab (e.g. Hanafi, etc.)
* Fetches prayer times from an API (e.g. UmmahAPI)
* Parses and displays Fajr, Dhuhr, Asr, Maghrib, Isha times

## 🛠 Requirements

* .NET SDK (6, 7, 8, or whichever your project targets)
* Internet access (to call the external API)
* (Optional) API key if the API requires one

## 🚀 How to Run (Console)

1. Clone the repo:

   ```bash
   git clone https://github.com/SheriPrime/prayerTimings.git
   cd prayerTimings
   ```

2. Restore and build:

   ```bash
   dotnet restore
   dotnet build
   ```

3. Run:

   ```bash
   dotnet run
   ```

   Or run the compiled executable from `/bin/Debug/...` or `/bin/Release/...`.

## 📦 Publish for Windows & Linux (to share with your bestie)

You can create a single `.exe` (with .NET runtime bundled) so your Windows friend can run it without needing .NET installed:

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```
and if your friend uses linux,
```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
```

Then share the generated `.exe` from `bin/Release/netX.X/win-x64/publish/`.

## 🧩 Integration with APIs

In your code, you’ll:

1. Get user latitude/longitude (via IP geolocation or manual input)
2. Construct the API request URL (e.g. `https://api.ummahapi.com/api/prayer-times?lat={lat}&lng={lng}&madhab=Hanafi&method=MuslimWorldLeague`)
3. Send HTTP request using `HttpClient`
4. Parse the JSON response (extract `data` and prayer times)
