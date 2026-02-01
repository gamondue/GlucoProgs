# Windows Persistent Alarms - Implementation Guide

## Overview
Questo documento descrive come implementare allarmi persistenti su Windows che funzionano anche quando l'applicazione GlucoMan è chiusa.

## Opzioni di Implementazione

### Opzione 1: Windows Task Scheduler (RACCOMANDATO)

#### Vantaggi
- Nativo di Windows, molto affidabile
- Non richiede servizi aggiuntivi
- Gli allarmi sopravvivono al riavvio del sistema
- Supportato su tutte le versioni di Windows

#### Svantaggi
- Richiede permessi per creare task schedulati
- Ogni allarme = un task separato
- Gestione più complessa (creazione/cancellazione task)

#### Implementazione Base

```csharp
using System.Diagnostics;

public class WindowsTaskSchedulerAlarmScheduler : ISystemAlarmScheduler
{
    private const string TASK_PREFIX = "GlucoMan_Alarm_";
    
    public async Task ScheduleAsync(Alarm alarm)
    {
        if (!alarm.IdAlarm.HasValue) 
            throw new ArgumentException("Alarm must have ID");
            
        var taskName = $"{TASK_PREFIX}{alarm.IdAlarm}";
        var exePath = Environment.ProcessPath; // Path to GlucoMan.exe
        var arguments = $"/alarm {alarm.IdAlarm}"; // Launch args
        var triggerTime = alarm.NextTriggerTime ?? DateTime.Now;
        
        // Create XML for scheduled task
        var xml = CreateTaskXml(taskName, exePath, arguments, triggerTime);
        
        // Use schtasks.exe to create the task
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "schtasks.exe",
                Arguments = $"/Create /TN \"{taskName}\" /XML \"{xmlFile}\" /F",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        await process.WaitForExitAsync();
        
        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            throw new InvalidOperationException($"Failed to create task: {error}");
        }
    }
    
    public async Task CancelAsync(int idAlarm)
    {
        var taskName = $"{TASK_PREFIX}{idAlarm}";
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "schtasks.exe",
                Arguments = $"/Delete /TN \"{taskName}\" /F",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        await process.WaitForExitAsync();
    }
    
    private string CreateTaskXml(string taskName, string exePath, string args, DateTime triggerTime)
    {
        return $@"<?xml version=""1.0"" encoding=""UTF-16""?>
<Task version=""1.2"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
  <RegistrationInfo>
    <Description>GlucoMan Alarm Reminder</Description>
    <Author>GlucoMan</Author>
  </RegistrationInfo>
  <Triggers>
    <TimeTrigger>
      <StartBoundary>{triggerTime:yyyy-MM-ddTHH:mm:ss}</StartBoundary>
      <Enabled>true</Enabled>
    </TimeTrigger>
  </Triggers>
  <Principals>
    <Principal>
      <LogonType>InteractiveToken</LogonType>
      <RunLevel>LeastPrivilege</RunLevel>
    </Principal>
  </Principals>
  <Settings>
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
    <AllowHardTerminate>true</AllowHardTerminate>
    <StartWhenAvailable>true</StartWhenAvailable>
    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
    <AllowStartOnDemand>true</AllowStartOnDemand>
    <Enabled>true</Enabled>
    <Hidden>false</Hidden>
    <RunOnlyIfIdle>false</RunOnlyIfIdle>
    <WakeToRun>true</WakeToRun>
    <ExecutionTimeLimit>PT1H</ExecutionTimeLimit>
    <Priority>7</Priority>
  </Settings>
  <Actions Context=""Author"">
    <Exec>
      <Command>{exePath}</Command>
      <Arguments>{args}</Arguments>
    </Exec>
  </Actions>
</Task>";
    }
}
```

#### Modifiche necessarie a MauiProgram.cs

Gestire i parametri di avvio per mostrare l'allarme:

```csharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Check for alarm launch argument
        var args = Environment.GetCommandLineArgs();
        if (args.Length > 1 && args[0] == "/alarm")
        {
            if (int.TryParse(args[1], out int alarmId))
            {
                // Show alarm immediately
                MainPage = new NavigationPage(new AlarmTriggeredPage(alarmId));
                return;
            }
        }

        MainPage = new AppShell();
    }
}
```

### Opzione 2: Windows Service

#### Vantaggi
- Esecuzione continua in background
- Non dipende dall'app principale
- Può gestire allarmi complessi

#### Svantaggi
- Richiede installazione come servizio Windows (privilegi admin)
- Comunicazione IPC tra servizio e app
- Più complesso da deployare

#### Implementazione (Schema)

1. **Creare un progetto Windows Service separato**
   ```
   GlucoMan.WindowsService/
   ??? AlarmService.cs
   ??? AlarmChecker.cs
   ??? IPC/
       ??? NamedPipeServer.cs
   ```

2. **Comunicazione tramite Named Pipes o WCF**
   - L'app MAUI invia comandi al servizio
   - Il servizio monitora gli allarmi
   - Il servizio lancia l'app quando scatta un allarme

3. **Installazione**
   ```powershell
   sc.exe create GlucoManAlarmService binPath= "C:\Path\To\GlucoMan.WindowsService.exe"
   sc.exe start GlucoManAlarmService
   ```

### Opzione 3: System Tray Application

#### Vantaggi
- Sempre in esecuzione ma minimizzata
- Non richiede permessi speciali
- Facile da implementare

#### Svantaggi
- Usa memoria costantemente
- L'utente deve lasciarlo in esecuzione

#### Implementazione

Modificare `MauiProgram.cs` per aggiungere icona system tray:

```csharp
#if WINDOWS
using Microsoft.UI.Xaml;
using H.NotifyIcon; // NuGet package

public partial class App : Application
{
    private TaskbarIcon? _notifyIcon;
    
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        
        // Create system tray icon
        _notifyIcon = new TaskbarIcon
        {
            IconSource = new BitmapImage(new Uri("ms-appx:///Assets/icon.ico")),
            ToolTipText = "GlucoMan - Alarms Active"
        };
        
        _notifyIcon.LeftClick += (s, e) => window.Activate();
        
        // Hide to tray instead of closing
        window.Destroying += (s, e) =>
        {
            e.Cancel = true; // Prevent close
            window.Hide();
        };
        
        return window;
    }
}
#endif
```

## Scelta Consigliata

Per GlucoMan, **Opzione 1 (Task Scheduler)** è la migliore perché:
1. Non richiede servizi aggiuntivi
2. Allarmi sopravvivono ai riavvii
3. Minimo impatto sulle risorse
4. Nativo di Windows

## Passi di Implementazione (Task Scheduler)

1. ? Creare classe `WindowsTaskSchedulerAlarmScheduler`
2. ? Implementare creazione/cancellazione task
3. ? Modificare App.xaml.cs per gestire parametri di avvio
4. ? Creare `AlarmTriggeredPage` per visualizzazione immediata
5. ? Testare con diversi scenari:
   - App chiusa
   - App in esecuzione
   - Computer riavviato
   - Sospensione/ripristino

## Testing

```powershell
# Verificare task creati
schtasks /query /FO LIST /TN "GlucoMan_Alarm_*"

# Eseguire task manualmente per test
schtasks /run /TN "GlucoMan_Alarm_123"

# Eliminare tutti i task GlucoMan
schtasks /query /FO CSV | findstr GlucoMan | foreach { schtasks /delete /TN $_ /F }
```

## Considerazioni di Sicurezza

- Validare sempre l'ID allarme dai parametri di avvio
- Limitare la durata di esecuzione dei task (ExecutionTimeLimit)
- Non memorizzare informazioni sensibili nei task XML
- Pulire task obsoleti periodicamente

## Alternative per il Futuro

Se si migra da .NET MAUI a Windows App SDK, si può usare:
- `BackgroundTaskBuilder` con `TimeTrigger`
- Toast notifications schedulate
- Background tasks nativi

## Risorse

- [Windows Task Scheduler XML Schema](https://docs.microsoft.com/en-us/windows/win32/taskschd/task-scheduler-schema)
- [schtasks Command Reference](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/schtasks)
- [.NET Process Class](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process)

---

**Nota:** Questa implementazione richiede test approfonditi prima del deployment in produzione.
