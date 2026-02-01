# GlucoMan - Sistema Allarmi Windows

## Stato Attuale dell'Implementazione

Il sistema di allarmi per Windows è stato implementato con le seguenti caratteristiche:

### ? Funzionalità Implementate

1. **Interfaccia Allarme Visibile**
   - Schermata full-screen rossa con testo chiaro
   - Icona orologio visibile (?)
   - Orologio in tempo reale aggiornato ogni secondo
   - Effetto lampeggiante per attirare l'attenzione
   - Messaggio dell'allarme ben visibile

2. **Suoni e Notifiche**
   - Beep sonori ripetuti ogni 3 secondi
   - 3 beep consecutivi ad alta frequenza (1400 Hz)

3. **Controlli Utente**
   - Pulsante **SNOOZE** (posticipa di 5 minuti)
   - Pulsante **DISMISS** (elimina l'allarme)
   - Impossibile chiudere accidentalmente con back button

4. **Gestione Finestre**
   - La finestra dell'app viene portata automaticamente in primo piano quando scatta l'allarme
   - Utilizza API Windows native per attivare la finestra

### ?? Limitazioni Attuali

#### **IMPORTANTE: L'app DEVE essere in esecuzione**

L'implementazione attuale utilizza timer in-memory che **richiedono che l'applicazione GlucoMan sia in esecuzione**. Gli allarmi NON funzioneranno se:
- L'applicazione è stata completamente chiusa
- Windows ha terminato il processo
- Il computer è in modalità sospensione

Questo è dovuto alle limitazioni di .NET MAUI su Windows che non supporta nativamente i background task come le app UWP.

### ?? Workaround Disponibili

Per mantenere gli allarmi attivi anche quando l'app principale è chiusa, ci sono diverse opzioni:

#### Opzione 1: Minimizzare invece di Chiudere ? CONSIGLIATO
- Minimizzare l'app nella system tray invece di chiuderla
- L'app continuerà ad eseguire in background
- Gli allarmi funzioneranno normalmente

#### Opzione 2: Implementazione Future (in Development)
Stiamo valutando le seguenti soluzioni:

1. **Windows Task Scheduler Integration**
   - Creare task schedulati di Windows per ogni allarme
   - I task avvieranno l'app con parametri specifici all'orario dell'allarme
   - Pro: Funziona anche con app chiusa
   - Contro: Richiede permessi amministrativi per alcuni scenari

2. **Windows Service (Richiede app separata)**
   - Creare un servizio Windows separato che gestisce gli allarmi
   - Il servizio comunica con l'app principale via IPC
   - Pro: Massima affidabilità
   - Contro: Installazione più complessa, richiede privilegi elevati

3. **System Tray Application**
   - Convertire GlucoMan in un'app che risiede nella system tray
   - L'app rimane sempre attiva ma minimizzata
   - Pro: Implementazione più semplice
   - Contro: Usa memoria anche quando non in uso attivo

## Come Utilizzare gli Allarmi

### Creare un Nuovo Allarme

1. Aprire la pagina **Alarms** dal menu principale
2. Compilare i campi:
   - **Reminder Text**: Messaggio da visualizzare
   - **Start Date/Time**: Quando deve scattare l'allarme
   - **Trigger Interval**: Tempo di validità (opzionale)
   - **Interval**: Intervallo di ripetizione (opzionale)
   - **Max Repeat Count**: Numero massimo di ripetizioni
3. Abilitare **Play Sound** per i beep sonori
4. Cliccare **ADD** per salvare e attivare l'allarme

### Testare il Sistema

- Utilizzare il pulsante **TEST** per creare un allarme di prova che scatta dopo 5 secondi
- Verificare che l'app sia in esecuzione quando l'allarme dovrebbe scattare

### Visualizzazione Allarmi

Filtri disponibili:
- **Active**: Solo allarmi attivi e futuri
- **Expired**: Allarmi scaduti
- **Show All**: Tutti gli allarmi in un intervallo di date

## Raccomandazioni d'Uso

### Per Uso Affidabile degli Allarmi:

1. ? **Mantenere l'app in esecuzione**
   - Non chiudere completamente l'app
   - Minimizzarla quando non in uso

2. ? **Verificare gli allarmi pianificati**
   - Controllare la lista degli allarmi attivi
   - Il campo "Next Trigger Time" mostra quando scatterà

3. ? **Testare prima di situazioni critiche**
   - Usare la funzione TEST per verificare
   - Controllare i log in caso di problemi

4. ?? **Evitare**
   - Chiudere completamente l'app se ci sono allarmi attivi
   - Mettere il PC in sospensione all'orario dell'allarme
   - Terminare il processo da Task Manager

## Log e Troubleshooting

I log degli allarmi sono salvati in:
- `%UserProfile%\GlucoMan\GlucoMan_Log.txt` - Eventi generali
- `%UserProfile%\GlucoMan\GlucoMan_Debug.txt` - Debug dettagliato
- `%UserProfile%\GlucoMan\GlucoMan_Errors.txt` - Errori

Cercare linee contenenti `"SystemAlarmScheduler"` per debug specifico degli allarmi.

### Problemi Comuni

**L'allarme non è scattato:**
- Verificare nei log se l'allarme era stato schedulato
- Controllare che l'app fosse in esecuzione
- Verificare che l'orario del sistema fosse corretto

**L'allarme scatta ma non è visibile:**
- Controllare che non ci siano altre finestre fullscreen aperte
- Verificare i log per messaggi di errore
- Provare a portare manualmente l'app in primo piano

**Nessun suono:**
- I beep usano l'altoparlante del PC (non sempre disponibile)
- Verificare che il volume di sistema sia attivo
- Alcuni sistemi potrebbero non supportare Console.Beep()

## Roadmap Futura

- [ ] Implementazione system tray
- [ ] Integrazione con Windows Task Scheduler
- [ ] Opzione per allarmi persistenti (anche con app chiusa)
- [ ] Supporto per file audio personalizzati
- [ ] Vibrazione tramite dispositivi supportati
- [ ] Notifiche toast native di Windows come backup

## Note per Sviluppatori

L'implementazione si trova in:
- `GlucoMan.Maui\Platforms\Windows\SystemAlarmScheduler.cs` - Logica scheduling
- `GlucoMan.Maui\WindowsAlarmPage.xaml(.cs)` - UI della pagina allarme
- `GlucoMan.Maui\Platforms\Windows\NotificationHelper.cs` - Helper notifiche
- `GlucoMan.Maui\AlarmSyncHelper.cs` - Sincronizzazione allarmi

Per estendere la funzionalità, considerare l'integrazione con:
- Windows App SDK Notifications
- Background Tasks (se migrato a Windows App SDK)
- COM per integrazione con Task Scheduler

---

**Data Ultima Modifica:** Dicembre 2024  
**Versione GlucoMan:** .NET 9 MAUI
