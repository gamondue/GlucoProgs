using Microsoft.Maui.Devices.Sensors;

namespace GlucoMan.Maui.Services;

/// <summary>
/// Simulates GPS movement for debugging on Windows.
/// Walks through a list of waypoints, advancing one step per call.
/// The real IGeoTimeZoneService (GeoTimeZoneService) works on these coordinates
/// and will return the correct IANA timezone + DST flag automatically.
/// Usage: inject into DefaultBackgroundGpsService via DI (see MauiProgram.cs).
/// </summary>
public class MockGeolocation : IGeolocation
{
    private int _index = 0;

    // Customise these waypoints to simulate your route.
    // Default: a short walk around Milan city centre.
    private static readonly (double Lat, double Lon, double? Alt)[] Waypoints =
    [
        (45.4654, 9.1859, 122),   // Duomo di Milano
        (45.4658, 9.1875, 122),
        (45.4663, 9.1891, 123),
        (45.4670, 9.1905, 123),   // Galleria Vittorio Emanuele
        (45.4675, 9.1920, 124),
        (45.4681, 9.1935, 124),
        (45.4688, 9.1950, 125),   // Teatro alla Scala
        (45.4695, 9.1965, 125),
        (45.4700, 9.1980, 126),
        (45.4706, 9.1995, 126),
    ];

    /// <summary>
    /// Returns the next waypoint in the sequence (loops around).
    /// </summary>
    public Task<Location> GetLocationAsync(GeolocationRequest request, CancellationToken cancelToken)
    {
        var (lat, lon, alt) = Waypoints[_index % Waypoints.Length];
        _index++;

        var location = new Location(lat, lon)
        {
            Altitude = alt,
            Accuracy = 5.0,
            Speed = 1.4,          // ~5 km/h walking speed
            Timestamp = DateTimeOffset.Now,
        };

        return Task.FromResult(location);
    }

    public Task<Location> GetLocationAsync(GeolocationRequest request)
        => GetLocationAsync(request, CancellationToken.None);

    public Task<Location> GetLastKnownLocationAsync()
    {
        var (lat, lon, alt) = Waypoints[0];
        return Task.FromResult(new Location(lat, lon) { Altitude = alt });
    }

    public bool IsListeningForeground => false;
    public bool IsEnabled => true;

    public event EventHandler<GeolocationLocationChangedEventArgs> LocationChanged { add { } remove { } }
    public event EventHandler<GeolocationListeningFailedEventArgs> ListeningFailed { add { } remove { } }

    public Task<bool> StartListeningForegroundAsync(GeolocationListeningRequest request)
        => Task.FromResult(false);

    public void StopListeningForeground() { }
}
