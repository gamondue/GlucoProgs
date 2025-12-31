# QUICK FIX - GPS "Waiting for GPS..." Problem

## ?? AZIONE RICHIESTA DA TE

### 1. Apri AndroidManifest.xml
File: `GlucoMan.Maui/Platforms/Android/AndroidManifest.xml`

### 2. Trova questa riga:
```xml
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
```

### 3. AGGIUNGI subito dopo queste righe:
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

### 4. AGGIUNGI il servizio dentro `<application>`:
Trova il tag `<application ...>` e aggiungi PRIMA del tag di chiusura `</application>`:

```xml
<!-- GPS Tracking Service for background location -->
<service 
    android:name=".GpsTrackingService"
    android:enabled="true"
    android:exported="false"
    android:foregroundServiceType="location" />
```

### 5. DISINSTALLA e REINSTALLA l'app:
```
1. Chiudi l'app su Android
2. Visual Studio > Build > Rebuild Solution
3. Android: Impostazioni > App > GlucoMan > DISINSTALLA
4. Visual Studio > Debug > Start Debugging
```

## ? Cosa è già stato fatto automaticamente

- ? `MainApplication.cs` - Permessi dichiarati con attributi assembly
- ? `MainPage.xaml.cs` - Richiesta permesso LocationWhenInUse all'avvio
- ? `TrackPage.xaml.cs` - Richiesta permesso LocationAlways per background tracking
- ? Documentazione completa in `ANDROID_PERMISSIONS_GPS.md`

## ?? Test dopo la reinstallazione

1. Apri GlucoMan ? concedi "Consenti durante l'uso"
2. Physical Activity ? GPS Track ? concedi "Consenti sempre"
3. Verifica che NON compaia più "Waiting for GPS..."
4. Premi Home ? verifica che tracking continui (vedrai notifica persistente)

## ?? Posizione finale nel AndroidManifest.xml

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
	<!-- Basic permissions -->
	<uses-permission android:name="android.permission.INTERNET" />
	<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	
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
	
	<!-- Camera, storage, etc... (existing permissions) -->
	...
	
	<application ...>
		<!-- Existing providers... -->
		
		<!-- GPS Tracking Service for background location -->
		<service 
			android:name=".GpsTrackingService"
			android:enabled="true"
			android:exported="false"
			android:foregroundServiceType="location" />
	</application>
</manifest>
```

## ? FAQ Rapide

**Q: Perché devo disinstallare?**
A: Android legge i permessi del manifest SOLO durante installazione. Update non basta.

**Q: Come faccio a sapere se funziona?**
A: Quando apri GPS Track, vedrai "GPS permission granted (including background)" in verde invece di "Waiting for GPS...".

**Q: Il tracking si ferma quando vado in background**
A: Assicurati di aver concesso "Consenti sempre" quando richiesto, non "Solo durante l'uso".

**Q: Vedo una notifica "GPS Tracking in corso..."**
A: Normale! È obbligatoria su Android quando un servizio usa GPS in background. Scomparirà quando fermi il tracking.
