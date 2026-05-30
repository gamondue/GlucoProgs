# Windows Alarm Precision Improvements

## Problema Identificato

Gli allarmi su Windows presentavano una precisione estremamente scarsa:
- Ritardi di 20+ secondi al primo trigger
- Allarmi periodici che saltavano completamente le attivazioni successive
- Imprecisione dovuta all'utilizzo di `System.Timers.Timer` che è notoriamente impreciso per intervalli lunghi

## Soluzione Implementata

È stato implementato un **approccio a due fasi** per bilanciare efficienza energetica e precisione:

### Fase 1: Modalità Low-Power (Timer Impreciso)
- Utilizza `System.Timers.Timer` standard
- Attiva fino a **1 minuto prima** del momento di trigger dell'allarme
- Consumo energetico minimo
- Precisione non critica (± secondi)

### Fase 2: Modalità High-Precision (Active Polling)
- Si attiva automaticamente nell'ultimo minuto prima del trigger
- Utilizza un loop di attesa attiva con `Task.Delay`
- **Sleep adattivo** per ottimizzare CPU vs precisione:
  - `1000ms` quando mancano >10 secondi
  - `100ms` quando mancano 2-10 secondi  
  - `10ms` quando mancano <2 secondi
- Precisione sub-secondo (tipicamente <100ms di ritardo)

### Dettagli Implementativi

```csharp
// Costante configurabile per il threshold di precisione
private static readonly TimeSpan PrecisionModeThreshold = TimeSpan.FromMinutes(1);

// Gestione di timer imprecisi E task ad alta precisione
private static readonly Dictionary<int, SystemTimer> _activeTimers = new();
private static readonly Dictionary<int, CancellationTokenSource> _activePrecisionTasks = new();
```

#### Flusso di Scheduling

1. **Calcolo tempo rimanente**: `timeUntilAlarm = triggerTime - DateTime.Now`

2. **Decisione strategia**:
   - Se `timeUntilAlarm <= 1 minuto`: avvia subito modalità precisione
   - Altrimenti: avvia timer impreciso per `(timeUntilAlarm - 1 minuto)`

3. **Transizione automatica**: Quando il timer scatta, cancella se stesso e avvia `StartPrecisionMode()`

4. **Loop di precisione**:
   ```csharp
   while (!cancellationToken.IsCancellationRequested)
   {
	   var remaining = triggerTime - DateTime.Now;
	   if (remaining.TotalMilliseconds <= 0)
	   {
		   OnAlarmTriggered(alarm);
		   break;
	   }
	   await Task.Delay(adaptiveSleepMs, cancellationToken);
   }
   ```

### Logging Migliorato

Ogni fase logga eventi dettagliati per il debugging:
- Scheduling iniziale con strategia scelta
- Transizione a modalità precisione
- Trigger effettivo con timestamp precisi (HH:mm:ss.fff)
- Misurazione del ritardo effettivo in millisecondi

Esempio di log:
```
Windows SystemAlarmScheduler: Alarm 123 using two-phase approach: low-power for 14.5 min, then precision mode
Windows SystemAlarmScheduler: Alarm 123 switching to precision mode
Windows SystemAlarmScheduler: Precision mode started for alarm 123, target: 14:30:00.000
Windows SystemAlarmScheduler: Precision trigger for alarm 123 at 14:30:00.047 (scheduled: 14:30:00.000, delay: -47ms)
```

### Gestione della Cancellazione

Il metodo `CancelScheduledAlarm()` è stato esteso per gestire entrambe le fasi:
- Cancella timer imprecisi attivi
- Cancella task di precisione attivi tramite `CancellationTokenSource`
- Thread-safe con lock appropriati

### Conteggio Allarmi

`GetScheduledAlarmsCount()` ora conta sia timer che precision tasks:
```csharp
return _activeTimers.Count + _activePrecisionTasks.Count;
```

## Benefici

✅ **Precisione migliorata drasticamente**: da ±20s a <100ms  
✅ **Affidabilità**: nessun allarme saltato  
✅ **Efficienza energetica**: CPU usage elevato solo nell'ultimo minuto  
✅ **Scalabilità**: gestisce correttamente allarmi multipli simultanei  
✅ **Osservabilità**: logging dettagliato per troubleshooting  

## Note Tecniche

- **Thread-safety**: Tutti gli accessi alle collezioni condivise sono protetti da `lock (_lock)`
- **Cancellazione cooperativa**: Utilizza `CancellationToken` per terminare gracefully i task
- **Memoria**: I `CancellationTokenSource` vengono disposti correttamente dopo l'uso
- **Eccezioni**: `OperationCanceledException` gestita come flow control normale

## Testing

Per testare la precisione:
1. Impostare un allarme tra 2-3 minuti
2. Osservare nei log il passaggio a modalità precisione dopo 1-2 minuti
3. Verificare il timestamp effettivo del trigger vs quello schedulato
4. Confermare che il ritardo sia <500ms

## Possibili Estensioni Future

- Rendere `PrecisionModeThreshold` configurabile dall'utente
- Aggiungere metriche statistiche (ritardo medio, max, percentili)
- Modalità "ultra-precision" opzionale con sleep ancora più brevi
- Fallback a Windows Task Scheduler per allarmi quando app chiusa
