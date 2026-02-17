# Quick Start - Test Sistema Allarmi Windows

## ?? Come Testare Subito gli Allarmi

### Prerequisiti
- ? GlucoMan compilato e avviato su Windows
- ? Database inizializzato

### Test Rapido (5 secondi)

1. **Avvia GlucoMan** su Windows
2. **Naviga** alla pagina "Alarms" dal menu
3. **Leggi** il banner giallo di avviso (importante!)
4. **Compila solo questi campi:**
   - Reminder Text: `Test Alarm - Prova`
   - Abilita "Play Sound" (checkbox)
5. **Clicca** il pulsante **TEST**
6. **Attendi 5 secondi**

### Cosa Dovresti Vedere

Dopo 5 secondi:
1. ?? Schermata rossa fullscreen appare
2. ? Icona orologio grande al centro
3. ?? Orario corrente aggiornato ogni secondo
4. ?? Messaggio "Test Alarm - Prova" evidenziato
5. ?? 3 beep sonori ripetuti ogni 3 secondi
6. ?? Sfondo lampeggia tra rosso chiaro e scuro
7. ?? Due pulsanti grandi: SNOOZE e DISMISS

### Test Azioni

#### Test DISMISS
1. Clicca **DISMISS**
2. ? Allarme sparisce
3. ? Beep si fermano
4. ? Torni alla pagina precedente
5. ? Nel database l'allarme è marcato come "Dismissed"

#### Test SNOOZE
1. Crea un nuovo test alarm
2. Attendi che scatti
3. Clicca **SNOOZE 5 MINUTES**
4. ? Allarme sparisce
5. ? Viene creato nuovo allarme per +5 minuti
6. ? Aspetta 5 minuti ? allarme scatta di nuovo

### Test Allarme Programmato

1. **Crea un allarme reale:**
   ```
   Reminder Text: Misura glicemia
   Start Date: [oggi]
   Start Time: [tra 2 minuti da ora]
   Trigger Interval: 300 (5 minuti di validità)
   ```

2. **Clicca** ADD

3. **Verifica** nella lista che appaia con stato "Active"

4. **Attendi** 2 minuti

5. **Osserva** l'allarme che scatta automaticamente

### Test Scenario Reale: Insulin Reminder

```
Reminder Text: Insulina basale serale
Start Date: [oggi]
Start Time: 21:00
Interval: 86400 (1 giorno in secondi)
Duration: 86400
Max Repeat Count: 365
Play Sound: ?
```

Questo creerà un promemoria giornaliero alle 21:00 per un anno.

## ?? Test Avanzati

### Test 1: Multipli Allarmi Simultanei

1. Crea 3 test alarms a 5, 10, 15 secondi
2. Tutti dovrebbero scattare
3. Verifica che ogni allarme mostri il messaggio corretto

### Test 2: Finestra in Background

1. Crea test alarm (5 secondi)
2. Minimizza GlucoMan
3. Apri altre app (browser, etc)
4. ? Dopo 5 secondi, GlucoMan dovrebbe tornare in primo piano

### Test 3: Allarme con App Minimizzata (30 min)

1. Crea allarme tra 30 minuti
2. Minimizza GlucoMan (NON chiudere!)
3. Continua a lavorare normalmente
4. ? Dopo 30 minuti, allarme dovrebbe scattare

### Test 4: CRITICAL - App Chiusa (dovrebbe FALLIRE)

?? **Questo test DEVE fallire** (limitazione nota):

1. Crea test alarm tra 1 minuto
2. **Chiudi completamente** GlucoMan (X sulla finestra)
3. Attendi 1 minuto
4. ? Allarme NON scatta (comportamento atteso)
5. Riapri GlucoMan
6. L'allarme dovrebbe essere marcato come "missed"

## ?? Checklist Funzionalità

Verifica che tutto funzioni:

- [ ] Creazione allarme
- [ ] Salvataggio in database
- [ ] Lista allarmi (Active/Expired/All)
- [ ] Modifica allarme esistente
- [ ] Cancellazione allarme
- [ ] Test alarm (5 secondi)
- [ ] Allarme scatta all'ora programmata
- [ ] Schermata rossa fullscreen appare
- [ ] Orologio si aggiorna
- [ ] Beep sonori funzionano
- [ ] Effetto lampeggiante visibile
- [ ] Pulsante DISMISS funziona
- [ ] Pulsante SNOOZE funziona (crea nuovo allarme +5min)
- [ ] Finestra va in primo piano
- [ ] Banner giallo di avviso visibile
- [ ] Logs vengono scritti correttamente

## ?? Se Qualcosa Non Funziona

### Nessun beep sonoro?
- Normale su alcune VM o sistemi senza speaker
- Prova con cuffie/speaker esterni
- Verifica volume Windows

### Allarme non appare?
1. Controlla i logs: `%UserProfile%\GlucoMan\GlucoMan_Log.txt`
2. Cerca righe con "SystemAlarmScheduler"
3. Verifica che l'orario di sistema sia corretto
4. Assicurati che l'app sia in esecuzione

### Finestra non va in primo piano?
- Può dipendere da impostazioni Windows (Focus Assist)
- Prova a disabilitare "Focus Assist" durante i test

### Allarme non appare con app chiusa?
- ? Comportamento corretto! È la limitazione nota
- Leggi `README_ALARMS.md` per le soluzioni future

## ?? Dove Guardare i Logs

Apri Esplora File e vai a:
```
%UserProfile%\GlucoMan\
```

File importanti:
- `GlucoMan_Log.txt` - Eventi generali
- `GlucoMan_Debug.txt` - Debug dettagliato allarmi
- `GlucoMan_Errors.txt` - Solo errori

Cerca queste stringhe:
```
"SystemAlarmScheduler: Scheduling alarm"
"SystemAlarmScheduler: Alarm triggered"
"Shown fullscreen alarm page"
```

## ? Configurazione Consigliata per Uso Reale

1. **NON chiudere mai l'app** - Solo minimizzare
2. **Impostare allarmi importanti** con suono
3. **Testare sempre** un giorno prima di eventi critici
4. **Controllare "Active Alarms"** regolarmente
5. **Pulire allarmi scaduti** periodicamente

## ?? Scenari d'Uso Raccomandati

### Reminder Misurazione Glicemia
```
Reminder: "Misura glicemia pre-pranzo"
Time: 12:00
Interval: 86400 (daily)
Sound: ON
```

### Reminder Insulina
```
Reminder: "Insulina basale"
Time: 22:00
Interval: 86400 (daily)
Duration: 3600 (1 ora di validità)
Sound: ON
Max Repeat: 365
```

### Reminder Visita Medica
```
Reminder: "Visita diabetologo domani ore 10"
Time: [giorno prima, ore 18:00]
Sound: ON
(no interval - one-time)
```

## ?? Prossimi Test da Fare (Task Scheduler Implementation)

Quando verrà implementato Task Scheduler, testare:
- [ ] Allarme funziona con app chiusa
- [ ] Allarme funziona dopo riavvio PC
- [ ] Allarme funziona dopo sospensione/ripristino
- [ ] Task vengono creati in Windows Task Scheduler
- [ ] Task vengono cancellati correttamente
- [ ] Permessi sufficienti senza admin

---

**Happy Testing! ??**

Per domande o problemi, controlla:
1. `README_ALARMS.md` - Documentazione utente
2. `IMPLEMENTATION_PERSISTENT_ALARMS.md` - Guida sviluppatore
3. `WINDOWS_ALARMS_IMPLEMENTATION_SUMMARY.md` - Riepilogo completo
