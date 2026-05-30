# Simulazione del Punto Geografico GPS (Windows Debug)

## Panoramica

Durante il debug su **Windows**, il sistema GPS reale restituisce la posizione fisica della macchina
(o `null` se il servizio di localizzazione è disabilitato). Per simulare uno spostamento geografico
senza muoversi fisicamente, il progetto include un'implementazione mock di `IGeolocation` che
avanza lungo una sequenza di waypoint predefiniti.

Il meccanismo si attiva **solo in configurazione `DEBUG` su Windows** e non influenza le build
Android né le build `Release`.

---

## File coinvolti

| File | Ruolo |
|------|-------|
| `GlucoMan.Maui/Services/MockGeolocation.cs` | Implementazione mock di `IGeolocation` con waypoint simulati |
| `GlucoMan.Maui/Services/DefaultBackgroundGpsService.cs` | Servizio GPS che usa `IGeolocation` iniettato (non più lo statico `Geolocation.Default`) |
| `GlucoMan.Maui/MauiProgram.cs` | Registrazione condizionale del mock nella DI |

---

## Come funziona

1. In `MauiProgram.cs`, nella sezione `#elif WINDOWS` / `#if DEBUG`, viene registrato
   `MockGeolocation` come singleton di `IGeolocation`:

   ```csharp
   #elif WINDOWS
   #if DEBUG
       builder.Services.AddSingleton<IGeolocation, MockGeolocation>();
       //builder.Services.AddSingleton<IGeolocation>(_ => Geolocation.Default);
   #else
       builder.Services.AddSingleton<IGeolocation>(_ => Geolocation.Default);
   #endif
       builder.Services.AddSingleton<IBackgroundGpsService, DefaultBackgroundGpsService>();
   ```

2. `DefaultBackgroundGpsService` riceve `IGeolocation` via costruttore (con fallback a
   `Geolocation.Default` se non registrato):

   ```csharp
   public DefaultBackgroundGpsService(IGeolocation geolocation = null)
   {
       this.geolocation = geolocation ?? Geolocation.Default;
   }
   ```

3. Il timer GPS (ogni 10 secondi) chiama `geolocation.GetLocationAsync(...)`, che in modalità
   mock restituisce il waypoint successivo nella lista, avanzando di uno a ogni chiamata e
   ricominciando dall'inizio alla fine del percorso.

---

## Personalizzare il percorso simulato

Apri **`GlucoMan.Maui/Services/MockGeolocation.cs`** e modifica l'array `Waypoints`:

```csharp
private static readonly (double Lat, double Lon, double? Alt)[] Waypoints =
[
    (45.4654, 9.1859, 122),   // Duomo di Milano
    (45.4658, 9.1875, 122),
    (45.4663, 9.1891, 123),
    // ... aggiungi quanti punti vuoi
];
```

### Come trovare le coordinate

- **Google Maps**: apri la mappa, clic destro sul punto desiderato → *"Che cosa c'è qui?"* →
  le coordinate appaiono in basso nella schermata nel formato `latitudine, longitudine`.
- **OpenStreetMap**: apri [openstreetmap.org](https://www.openstreetmap.org), clic destro →
  *"Mostra indirizzo"*.

### Parametri aggiuntivi simulabili

Nel costruttore di ogni `Location` puoi impostare:

| Proprietà | Descrizione | Valore di default nel mock |
|-----------|-------------|---------------------------|
| `Accuracy` | Precisione in metri | `5.0` |
| `Speed` | Velocità in m/s (1.4 ≈ 5 km/h a piedi) | `1.4` |
| `Altitude` | Altitudine in metri | valore del waypoint |
| `Timestamp` | Istante della rilevazione | `DateTimeOffset.Now` |

---

## Passare al GPS reale durante il debug

In `MauiProgram.cs` commenta la riga del mock e decommenta quella di `Geolocation.Default`:

```csharp
#if DEBUG
    //builder.Services.AddSingleton<IGeolocation, MockGeolocation>();   // ← mock
    builder.Services.AddSingleton<IGeolocation>(_ => Geolocation.Default); // ← reale
#else
    builder.Services.AddSingleton<IGeolocation>(_ => Geolocation.Default);
#endif
```

---

## Comportamento per configurazione e piattaforma

| Configurazione | Piattaforma | GPS usato |
|----------------|-------------|-----------|
| `Debug` | Windows | `MockGeolocation` (waypoint simulati) |
| `Release` | Windows | `Geolocation.Default` (GPS reale) |
| `Debug` / `Release` | Android | `BackgroundGpsServiceAndroid` (GPS reale) |

---

## Note tecniche

- `MockGeolocation` implementa l'intera interfaccia `IGeolocation` di .NET MAUI. I metodi
  non usati dal servizio GPS (`StartListeningForegroundAsync`, `StopListeningForeground`,
  `LocationChanged`, `ListeningFailed`) sono stub no-op.
- Il mock non richiede permessi di localizzazione dal sistema operativo Windows.
- L'intervallo di aggiornamento (10 secondi) è configurato nel timer di
  `DefaultBackgroundGpsService` ed è indipendente dal mock.
