# Metodo di Identificazione MISO con Due Dinamiche Separate

## 1. Modello del Sistema

Il modello rappresenta il sistema glicemico come somma di due funzioni di trasferimento del primo ordine indipendenti:

```
         k?                    k?
G?(s) = ????????      G?(s) = ????????
        ??·s + 1              ??·s + 1

y(t) = G?(s)·u?(t) + G?(s)·u?(t)
```

Dove:
- **u?(t)**: Carboidrati (CHO) come impulsi non periodici ?(t)
- **u?(t)**: Insulina come impulsi non periodici ?(t)
- **y(t)**: Glucosio misurato periodicamente (ogni 6 minuti)
- **k?, k?**: Guadagni statici dei due ingressi
- **??, ??**: Costanti di tempo (in secondi) delle due dinamiche
- **d?, d?**: Ritardi puri (dead time) dei due ingressi

## 2. Discretizzazione del Modello

Con campionamento uniforme a periodo **T? = 360 s** (6 minuti), ogni dinamica continua viene discretizzata:

```
       k·(1 - a)
G(z) = ?????????
         z - a
```

dove **a = exp(-T?/?)** è il polo discreto.

Per ogni ingresso i-esimo, la risposta al gradino è:

```
y[k] = k·(1 - a?)  per k ? 0
```

Il modello discreto completo ARX diventa:

```
y[k] = a?·y?[k-1] + a?·y?[k-1] + b?·u?[k-d?] + b?·u?[k-d?] + c + e[k]
```

Dove:
- **y?[k]**: Contributo del canale CHO al tempo k
- **y?[k]**: Contributo del canale Insulina al tempo k
- **a? = exp(-T?/??)**, **a? = exp(-T?/??)**: Poli discreti
- **b? = k?·(1-a?)**, **b? = k?·(1-a?)**: Guadagni modificati
- **c**: Offset (livello basale di glucosio)
- **e[k]**: Errore di modello (rumore)

## 3. Strategia di Identificazione

Dato che abbiamo **due dinamiche** indipendenti, l'identificazione simultanea di tutti i parametri è complessa. Utilizziamo una **strategia di separazione** basata su periodi con un solo ingresso attivo.

### 3.1 Filtraggio dei Dati

Cerchiamo due tipi di periodi:

**Tipo A - Solo CHO attivo:**
- Si verifica un evento CHO (pasto)
- Nessun evento insulina per almeno 7 ore prima e 7 ore dopo
- Durata minima: 4 ore di dati di glucosio dopo l'evento CHO

**Tipo B - Solo Insulina attiva:**
- Si verifica un evento insulina (iniezione)
- Nessun evento CHO per almeno 7 ore prima e 7 ore dopo
- Durata minima: 4 ore di dati di glucosio dopo l'evento insulina

La finestra di 7 ore è scelta perché:
- Copre circa 3 costanti di tempo (assumendo ? ? 2-3 ore)
- Garantisce che l'effetto del precedente evento sia < 5% del valore di regime

### 3.2 Identificazione per Canale Singolo (SISO)

Per ogni periodo filtrato, identifichiamo:

```
y[k] = a·y[k-1] + b·u[k-d] + c + e[k]
```

Dove:
- **a**: Polo discreto (da cui si ricava ? = -T?/ln(a))
- **b**: Guadagno modificato (da cui si ricava k = b/(1-a))
- **d**: Ritardo in campioni (delay)
- **c**: Offset

**Algoritmo di stima:**

1. **Grid search sul ritardo d** ? [0, 60] campioni (0-6 ore)
2. **Per ogni d**, risolvi il problema ai minimi quadrati:
   ```
   min ?(y[k] - a·y[k-1] - b·u[k-d] - c)²
   ```
3. Aggiungi **regolarizzazione ridge** per stabilità:
   ```
   min ?(y[k] - a·y[k-1] - b·u[k-d] - c)² + ?·(a² + b² + c²)
   ```
4. Scegli il **d che minimizza SSE** (Sum of Squared Errors)

### 3.3 Aggregazione dei Risultati

Per ogni canale (CHO e Insulina):
1. Identifica parametri su **N periodi** filtrati (tipo A o tipo B)
2. Ottieni N stime: {(a????, b????, c???, d????, SSE???)} per i=1..N
3. Calcola **statistiche** per ogni parametro:
   - Media: `? = (1/N)·? x?`
   - Deviazione standard: `? = sqrt((1/N)·?(x? - ?)²)`
   - Mediana: valore centrale dell'insieme ordinato
4. Riporta: **? ± ?** per ciascun parametro

### 3.4 Conversione a Parametri Fisici

Dai parametri discreti stimati, ricaviamo i parametri fisici:

**Costante di tempo (secondi):**
```
? = -T? / ln(a)
```

**Guadagno statico:**
```
k = b / (1 - a)
```

**Ritardo (secondi):**
```
T_delay = d · T?
```

## 4. Validazione del Modello

### 4.1 Metriche di Qualità

Per ogni identificazione calcoliamo:

**SSE (Sum of Squared Errors):**
```
SSE = ?(y_measured[k] - y_simulated[k])²
```

**MSE (Mean Squared Error):**
```
MSE = SSE / N_samples
```

**RMSE (Root Mean Squared Error):**
```
RMSE = sqrt(MSE)
```

**R² (Coefficient of Determination):**
```
R² = 1 - SSE / SST
dove SST = ?(y[k] - ?)²
```

### 4.2 Test di Simulazione

Per validare il modello identificato:
1. Simula la risposta su dati **non utilizzati** per l'identificazione
2. Confronta y_simulato con y_misurato
3. Calcola metriche di errore

## 5. Limitazioni e Assunzioni

### Assunzioni del Modello
1. **Linearità**: La risposta è proporzionale all'ingresso (valido per piccole variazioni)
2. **Tempo-invarianza**: I parametri non cambiano nel tempo
3. **Indipendenza**: Gli effetti di CHO e insulina si sommano linearmente
4. **Ritardo puro**: Il dead time è costante

### Limitazioni Pratiche
1. **Rumore di misura**: Il glucosio è affetto da rumore dei sensori
2. **Eventi misti**: Potrebbero esserci pochi periodi "puliti" con un solo ingresso
3. **Variabilità biologica**: I parametri possono variare tra giorni/settimane
4. **Fattori non modellati**: Stress, esercizio, sonno, ecc. non sono considerati

## 6. Interpretazione Fisiologica

### Canale CHO (Carboidrati)
- **??**: Tempo caratteristico di assorbimento dei carboidrati (tipicamente 1-3 ore)
- **k?**: Aumento di glucosio per grammo di CHO (mg/dL per g)
- **d?**: Ritardo digestivo prima dell'effetto sul glucosio

### Canale Insulina
- **??**: Tempo caratteristico di azione dell'insulina (tipicamente 2-4 ore)
- **k?**: Riduzione di glucosio per unità di insulina (mg/dL per U) - negativo!
- **d?**: Ritardo di assorbimento dell'insulina sottocutanea

### Offset c
- Rappresenta il livello basale di glucosio in assenza di ingressi
- Dovrebbe essere vicino alla glicemia a digiuno del paziente

## 7. Riferimenti Matematici

### Teoria dei Sistemi Lineari
- Oppenheim & Willsky, "Signals and Systems"
- Ljung, "System Identification: Theory for the User"

### Modelli di Glucosio
- Bergman Minimal Model
- Hovorka Model
- UVa/Padova T1D Simulator

### Regolarizzazione Ridge
- Tikhonov Regularization
- Ridge Regression (Hoerl & Kennard, 1970)

Two-channel (MISO) Identification Method

This document explains how to use `Identification2` to identify two separate first-order dynamics
(one for CHO and one for Insulin) from event-based data.

Workflow:
1. Collect meals, quick injections, and glucose records over a time interval.
2. Call `Identification2.FindIsolatedSegments` with appropriate parameters (isolation windows, basalCheckHours, etc.).
3. The function returns two lists: `isolatedMeals` and `isolatedInjections`. Each list contains events separated by `null` markers representing segment boundaries.
4. Call `Identification2.IdentifyTwoChannelsAsync` passing the isolated lists and parameters (TsSeconds, isolationHours, minDataHours, maxDelaySamples, ridge).
5. The function processes each segment, calls `IdentifySisoSegment` and aggregates valid results into `AggregatedResult` for CHO and Insulin.

Configuration notes:
- Use `debugMode` and `segmentInfoCallback` to inspect segments interactively during identification.
- Tune `minDataHours` and `isolationHours` based on the sampling density of your CGM.
