# Android GPS Permissions Configuration

## Problema Risolto
L'app mostrava "errore checking permission" e "Waiting for GPS..." quando si tentava di utilizzare il GPS tracking per le attività fisiche. Inoltre, il tracking si fermava quando l'app andava in background.

## Modifiche Implementate

### 1. MainApplication.cs ?
**COMPLETATO** - Gli attributi assembly sono stati aggiunti per dichiarare i permessi:

```csharp
// Permessi GPS per tracking attività fisiche
[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]

// Permessi per tracking in background (Android 10+)
[assembly: UsesPermission(Android.Manifest.Permission.AccessBackgroundLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.ForegroundService)]
// Android 14+ richiede questo permesso specifico per servizi in foreground con GPS
[assembly: UsesPermission(Android.Manifest.Permission.ForegroundServiceLocation)]
```

### 2. MainPage.xaml.cs ?
**COMPLETATO** - La richiesta runtime dei permessi base è stata aggiunta:

```csharp
// GPS/Location permissions for physical activity tracking
var PermissionLocation = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
if (PermissionLocation != PermissionStatus.Granted)
{
    PermissionLocation = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    General.LogOfProgram?.Event($"MainPage - Location permission requested: {PermissionLocation}");
}
```

### 3. TrackPage.xaml.cs ?
**COMPLETATO** - Richiesta esplicita dei permessi background con dialogo esplicativo:

La `TrackPage` ora:
1. Richiede prima il permesso `LocationWhenInUse` (base)
2. Poi richiede `LocationAlways` (background) con un dialogo che spiega perché serve
3. Se l'utente rifiuta il background, avvisa che il tracking funzionerà solo con app aperta
4. Registra tutti i permessi concessi/negati nei log

### 4. AndroidManifest.xml ??
**DA COMPLETARE MANUALMENTE** - Devi aggiungere i permessi GPS nel file XML.

## Come Completare la Configurazione

### Passo 1: Modifica AndroidManifest.xml (OBBLIGATORIO)

Apri il file `GlucoMan.Maui/Platforms/Android/AndroidManifest.xml` e **aggiungi queste righe subito dopo**:
```xml
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
```

**Righe da aggiungere:**
```xml
<!-- GPS/Location permissions for physical activity tracking -->
<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-feature android:name="android.hardware.location" android:required="false" />
<uses-feature android:name="android.hardware.location.gps" android:required="false" />
<uses-feature android:name="android.hardware.location.network" android:required="false" />

<!-- For background GPS tracking (Android 10+) -->
<uses-permission android:name="android.permission.ACCESS_BACKGROUND_LOCATION" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE_LOCATION" />
```

**IMPORTANTE**: Aggiungi anche la dichiarazione del servizio in foreground **dentro il tag `<application>`**:

```xml
<application ...>
    <!-- Existing providers... -->
    
    <!-- GPS Tracking Service for background location -->
    <service 
        android:name=".GpsTrackingService"
        android:enabled="true"
        android:exported="false"
        android:foregroundServiceType="location" />
</application>
```

### Passo 2: Ricompila e Reinstalla (OBBLIGATORIO)

?? **MOLTO IMPORTANTE**: I permessi nel manifest vengono letti SOLO durante l'installazione dell'app!

1. **Chiudi l'app** su Android se è in esecuzione
2. In Visual Studio: **Build > Rebuild Solution**
3. **DISINSTALLA completamente l'app** dal dispositivo Android:
   - Impostazioni > App > GlucoMan > Disinstalla
   - Oppure tieni premuto sull'icona app e trascina su "Disinstalla"
4. **Rideploya l'app**: **Debug > Start Debugging**

### Passo 3: Concedi i Permessi

Al primo avvio dopo la reinstallazione:

1. Apri GlucoMan
2. L'app chiederà il permesso di localizzazione ? **Concedi "Consenti solo durante l'uso dell'app"**
3. Vai su **"Physical Activity"**
4. Premi il pulsante **GPS Track** (icona tracking)
5. Apparirà un dialogo che spiega perché serve il background tracking
6. Se scegli **"Yes, Enable"**, Android chiederà il permesso background
7. Scegli **"Consenti sempre"** o **"Consenti solo durante l'uso dell'app"**

### Passo 4: Verifica Configurazione

#### Da Impostazioni Android:
1. **Impostazioni > App > GlucoMan > Autorizzazioni > Posizione**
2. Verifica che sia impostato su:
   - **"Consenti sempre"** (RACCOMANDATO per tracking in background)
   - oppure "Consenti solo durante l'uso dell'app" (tracking solo con app aperta)

#### Da Codice:
I log in `General.LogOfProgram` mostreranno:
```
TrackPage - Background location permission granted
```
oppure
```
TrackPage - Background location permission denied, only foreground available
```

## Come Funziona il Tracking in Background

### Con Permesso "Consenti sempre":
? Il tracking continua anche quando:
- Premi il pulsante Home (app in background)
- Blocchi lo schermo
- Passi ad altre app
- L'app viene chiusa (ma non terminata forzatamente)

### Con Permesso "Solo durante l'uso":
?? Il tracking si ferma quando:
- Premi Home e l'app va in background
- Blocchi lo schermo
- Passi ad altre app

### Servizio in Foreground:
Quando il tracking è attivo in background, vedrai:
- Una **notifica persistente** "GlucoMan GPS Tracking in corso..."
- Questa notifica è OBBLIGATORIA per Android (non può essere rimossa)
- Indica che l'app sta usando il GPS anche se non è visibile

## Troubleshooting

### "Waiting for GPS..." rimane bloccato

**Causa**: Permessi non concessi o GPS disabilitato

**Soluzione**:
1. Vai in **Impostazioni > Posizione** e attiva il GPS
2. Verifica permessi: **Impostazioni > App > GlucoMan > Autorizzazioni > Posizione**
3. Controlla i log: `General.LogOfProgram` mostra errori GPS
4. Prova all'**aperto** (il GPS non funziona bene al chiuso)

### Il tracking si ferma quando vado in background

**Causa**: Permesso "Consenti sempre" non concesso

**Soluzione**:
1. **Impostazioni > App > GlucoMan > Autorizzazioni > Posizione**
2. Cambia in **"Consenti sempre"**
3. Riapri l'app e riprova

### "Permission denied" anche dopo aver concesso

**Causa**: L'app è stata aggiornata senza reinstallazione completa

**Soluzione**:
1. **Disinstalla completamente** l'app (non solo "Cancella dati")
2. **Rebuild** del progetto in Visual Studio
3. **Reinstalla** l'app

### Android chiede di giustificare "Consenti sempre"

**Causa**: Android 11+ richiede una spiegazione per ACCESS_BACKGROUND_LOCATION

**Soluzione**: 
- Il dialogo in `TrackPage.CheckAndRequestLocationPermission()` fornisce già la spiegazione
- Quando Android chiede conferma, spiega: "Per registrare attività fisiche continue anche quando l'app è in background"

### La notifica "GPS Tracking" è fastidiosa

**Risposta**: La notifica è **OBBLIGATORIA** su Android 8+ quando un servizio in foreground è attivo.
- Non può essere disattivata (politica di sicurezza Android)
- Scomparirà automaticamente quando fermi il tracking
- Serve per trasparenza: l'utente sa che il GPS è in uso

## Note Importanti per Android 14+

Android 14 (API 34) e successivi richiedono:
1. ? Permesso `FOREGROUND_SERVICE_LOCATION` (già aggiunto)
2. ? Attributo `android:foregroundServiceType="location"` nel service (da aggiungere nel manifest)
3. ? Richiesta esplicita runtime di `LocationAlways` (già implementata)

## Flusso Completo dei Permessi

```
App Start
  ?
MainPage richiede LocationWhenInUse
  ?
Utente concede "Consenti durante l'uso"
  ?
Utente apre Physical Activity ? GPS Track
  ?
TrackPage verifica LocationWhenInUse (già concesso)
  ?
TrackPage richiede LocationAlways con dialogo esplicativo
  ?
Utente concede "Consenti sempre"
  ?
? Tracking funziona anche in background!
```

## Test Consigliati

1. **Test foreground**: Avvia tracking, tieni app aperta ? dovrebbe funzionare
2. **Test background**: Avvia tracking, premi Home ? tracking continua
3. **Test schermo bloccato**: Tracking attivo, blocca schermo ? tracking continua
4. **Test riavvio app**: Tracking attivo, chiudi e riapri app ? mostra dialog di recovery
5. **Test lunga distanza**: Cammina/corri 1-2 km ? verifica che tutti i punti siano salvati

## Privacy e Trasparenza

L'app:
- ? Spiega PRIMA di richiedere i permessi
- ? Registra tutti i permessi nei log
- ? Mostra notifica quando GPS attivo in background
- ? Permette all'utente di scegliere se vuole background tracking
- ? Funziona anche senza permesso Always (ma solo in foreground)
