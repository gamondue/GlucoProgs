User Guide — Two-channel Identification

From the app:
- Use the Statistics page and choose the appropriate date range.
- For advanced identification use the "Identify (two channels)" action which calls into `Identification2`.
- Parameters exposed in UI (or code):
  - Sampling period `TsSeconds` (s)
  - `basalCheckHours`, `maxBasalSlope` (for left-side check)
  - `beforeIsolationHours`, `isolationHours` (isolation windows)
  - `minDataHours` (minimum post-event data for a segment)
  - `maxDelaySamples`, `ridge` (identification parameters)

Debugging:
- Enable `debugMode` to show the chart of each segment during processing.
- The identification routine logs SISO parameters per segment and aggregated summaries to debug output.

Interpreting results:
- `AggregatedResult` contains mean/std/median for ?, K, Delay, Offset and quality metrics R² and RMSE.
- Discard channels with too few segments or low average R².

# Guida all'Utilizzo dell'Identificazione a Due Canali

## Panoramica

L'identificazione a due canali (metodo 2) permette di stimare **separatamente** i parametri dinamici dell'effetto dei carboidrati (CHO) e dell'insulina sul glucosio, ottenendo **due costanti di tempo** distinte invece di una singola come nel metodo 1.

## Differenze tra i Due Metodi

### Metodo 1: Identificazione MISO Unica Dinamica
**Modello:**
```
y(t) = G(s) * [u?(t) + u?(t)]
dove G(s) = 1/(?·s + 1)
```

**Caratteristiche:**
- Una sola costante di tempo ? condivisa
- Guadagni separati per CHO e Insulina (b?, b?)
- Ritardi separati (d?, d?)
- Identificazione simultanea su tutti i dati
- Più robusto ma meno preciso

**Risultati forniti:**
- a: coefficiente autoregressivo discreto
- b1: guadagno CHO modificato
- b2: guadagno insulina modificato
- c: offset
- Delay CHO, Delay Insulin
- ?: costante di tempo comune
- SSE: errore di fit

### Metodo 2: Identificazione MISO Due Dinamiche
**Modello:**
```
y(t) = G?(s)·u?(t) + G?(s)·u?(t)
dove:
  G?(s) = k?/(??·s + 1)  [canale CHO]
  G?(s) = k?/(??·s + 1)  [canale Insulina]
```

**Caratteristiche:**
- Due costanti di tempo separate ??, ??
- Guadagni statici k?, k?
- Ritardi separati d?, d?
- Identificazione su segmenti isolati
- Più preciso ma richiede dati puliti

**Risultati forniti (per ciascun canale):**
- ? [s]: costante di tempo (? ± ?)
- k: guadagno statico (? ± ?)
- Delay [samples]: ritardo (? ± ?)
- Offset: livello basale (? ± ?)
- R²: qualità del fit (media)
- # Segments: numero di segmenti trovati

## Come Funziona il Metodo 2

### 1. Ricerca di Segmenti Isolati

Il sistema cerca automaticamente periodi in cui:

**Per il canale CHO:**
- Si verifica un pasto (evento CHO)
- **Nessuna** iniezione di insulina nelle 7 ore precedenti e successive
- Nessun altro pasto nelle 7 ore precedenti e successive
- Almeno 4 ore di dati di glucosio disponibili dopo il pasto

**Per il canale Insulina:**
- Si verifica un'iniezione di insulina
- **Nessun** pasto nelle 7 ore precedenti e successive
- Nessun'altra iniezione nelle 7 ore precedenti e successive
- Almeno 4 ore di dati di glucosio disponibili dopo l'iniezione

### 2. Identificazione SISO per Ogni Segmento

Per ogni segmento isolato trovato:
1. Ricampiona il glucosio a 6 minuti (Ts = 360s)
2. Crea un impulso unitario all'evento di input
3. Esegue grid search sul ritardo (0-60 campioni = 0-6 ore)
4. Per ogni ritardo, risolve il problema ai minimi quadrati:
   ```
   y[k] = a·y[k-1] + b·u[k-d] + c
   ```
5. Seleziona il ritardo con SSE minimo
6. Calcola metriche di qualità (R², RMSE)

### 3. Aggregazione Statistica

Per ogni canale, i risultati di tutti i segmenti vengono aggregati:
- **Media (?)**: valore medio del parametro
- **Deviazione standard (?)**: dispersione dei valori
- **Mediana**: valore centrale (robusto agli outlier)

I risultati mostrati nell'interfaccia sono nel formato: **? ± ?**

## Interpretazione dei Risultati

### Costanti di Tempo (??, ??)

**?? (CHO)**: Velocità di assorbimento dei carboidrati
- Valori tipici: 3600-10800 s (1-3 ore)
- Valori più bassi: assorbimento rapido (zuccheri semplici)
- Valori più alti: assorbimento lento (carboidrati complessi)

**?? (Insulina)**: Velocità di azione dell'insulina
- Valori tipici: 7200-14400 s (2-4 ore)
- Dipende dal tipo di insulina (rapida, lenta)
- Più alta nei soggetti insulin-resistenti

### Guadagni Statici (k?, k?)

**k? (CHO)**: Aumento di glucosio per grammo di CHO
- Valori tipici: 2-5 mg/dL per g di CHO
- Positivo (i carboidrati aumentano il glucosio)
- Varia con sensibilità insulinica e stato di digiuno

**k? (Insulina)**: Riduzione di glucosio per unità di insulina
- Valori tipici: -30 a -100 mg/dL per U
- **Negativo** (l'insulina riduce il glucosio)
- Più negativo = maggiore sensibilità insulinica

### Ritardi (d?, d?)

**d? (CHO)**: Tempo prima che i carboidrati influenzino il glucosio
- Valori tipici: 2-10 campioni (12-60 minuti)
- Dipende da: tipo di CHO, presenza di grassi/proteine, svuotamento gastrico

**d? (Insulina)**: Tempo prima che l'insulina agisca
- Valori tipici: 3-15 campioni (18-90 minuti)
- Più lungo per insulina sottocutanea vs endovenosa

### Offset (c)

- Rappresenta il livello basale di glucosio
- Valori tipici: 80-120 mg/dL
- Dovrebbe essere vicino alla glicemia a digiuno

### R² (Coefficiente di Determinazione)

- Misura la qualità del fit (0 = pessimo, 1 = perfetto)
- Valori > 0.7: fit buono
- Valori > 0.8: fit ottimo
- Valori < 0.5: modello inadeguato o dati rumorosi

### Numero di Segmenti

- Indica quanti periodi isolati sono stati trovati
- **Minimo raccomandato**: 5 segmenti per canale
- Più segmenti = statistiche più affidabili
- Se < 3 segmenti: risultati poco affidabili

## Quando Usare Quale Metodo

### Usa Metodo 1 se:
- Vuoi una stima rapida dei parametri
- Hai pochi dati disponibili
- I pasti e le iniezioni sono frequenti e ravvicinati
- Ti interessa solo una stima approssimativa

### Usa Metodo 2 se:
- Vuoi distinguere le dinamiche di CHO e Insulina
- Hai abbondanza di dati (almeno 1 mese)
- Il paziente ha periodi regolari con solo pasti o solo iniezioni
- Vuoi parametri più accurati per la predizione
- Pianifichi di usare i parametri per controllo automatico

## Risoluzione Problemi

### "# Segments: 0" per un canale

**Cause possibili:**
1. Pasti e iniezioni troppo ravvicinati (< 7 ore)
2. Mancano dati di glucosio dopo gli eventi
3. Periodo di analisi troppo breve

**Soluzioni:**
- Analizza un periodo più lungo (2-3 mesi)
- Verifica che ci siano dati del sensore CGM
- Riduci `isolationHours` a 5-6 ore (meno rigoroso)

### Deviazione standard molto alta (? > ?)

**Cause:**
- Alta variabilità biologica giorno per giorno
- Eventi non omogenei (es. diversi tipi di pasti)
- Interferenze non modellate (stress, esercizio)

**Interpretazione:**
- Usa la **mediana** invece della media
- I parametri potrebbero non essere costanti nel tempo
- Considera analisi su sottogruppi (es. colazione vs cena)

### R² molto basso (< 0.5)

**Cause:**
- Modello troppo semplice per la complessità del sistema
- Rumore elevato nei dati
- Eventi di disturbo non modellati

**Possibili azioni:**
- Filtra i dati di glucosio prima dell'identificazione
- Escludere segmenti con eventi anomali
- Considerare modelli di ordine superiore

## Utilizzo dei Parametri Identificati

### Per la Predizione del Glucosio

Usa i parametri stimati per simulare la risposta futura:
```
y_future = simula(G?, G?, pasti_futuri, insulina_futura)
```

### Per il Calcolo dei Bolus

Inverti il modello per calcolare l'insulina necessaria:
```
insulina_necessaria = (glucosio_target - glucosio_attuale) / k?
```

### Per la Personalizzazione della Terapia

- Confronta i tuoi parametri con i valori tipici
- Identifica se sei più o meno sensibile all'insulina
- Adatta i rapporti insulina/carboidrati di conseguenza

## Validazione dei Risultati

### Criteri di Qualità

Un'identificazione è **affidabile** se:
1. Numero segmenti ? 5 per canale
2. R² medio > 0.7
3. Deviazione standard < 50% della media
4. Parametri fisicamente plausibili (vedi range sopra)

### Test di Simulazione

Dopo l'identificazione:
1. Simula la risposta su dati non usati per l'identificazione
2. Confronta predizione vs misure reali
3. Calcola errore di predizione (RMSE)
4. Se RMSE < 15 mg/dL: modello buono per predizione

## Riferimenti Tecnici

Per maggiori dettagli sul metodo matematico, vedi:
- `MISO_Two_Dynamics_Method.md`: Teoria matematica completa
- `Identification2.cs`: Implementazione del codice
- `Diagram MISO first order sum of dynamics.md`: Schema a blocchi del sistema
