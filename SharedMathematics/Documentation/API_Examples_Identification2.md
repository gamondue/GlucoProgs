# Esempi di Utilizzo dell'API Identification2

## Esempio Base: Identificazione Completa

```csharp
using Mathematics.Identification;
using GlucoMan.BusinessLayer;

// 1. Recupera i dati dal database
var blMeals = new BL_MealAndFood();
var blGlucose = new BL_GlucoseMeasurements();
var blInj = new BL_BolusesAndInjections();

var start = DateTime.Now.AddMonths(-2);
var end = DateTime.Now.AddMonths(-1);

var meals = blMeals.GetMeals(start, end);
var injections = blInj.GetInjections(start, end);
var glucose = blGlucose.GetSensorsRecords(start, end);

// 2. Converti a TimePoint[]
var mealPoints = Identification.FromMeals(meals);
var injPoints = Identification.FromInjections(injections);
var glucosePoints = Identification.FromGlucoseRecords(glucose);

// 3. Esegui identificazione
var (choResults, insulinResults) = Identification2.IdentifyTwoChannels(
    mealPoints,
    injPoints,
    glucosePoints,
    TsSeconds: 360.0,           // Campionamento 6 minuti
    isolationHours: 7.0,        // Finestra di isolamento 7 ore
    minDataHours: 4.0,          // Minimo 4 ore di dati dopo evento
    maxDelaySamples: 60,        // Ritardo massimo 6 ore
    ridge: 0.01);               // Regolarizzazione

// 4. Usa i risultati
if (choResults != null)
{
    Console.WriteLine($"Costante di tempo CHO: {choResults.TauMean:F0} ± {choResults.TauStd:F0} secondi");
    Console.WriteLine($"Guadagno CHO: {choResults.GainMean:F2} ± {choResults.GainStd:F2} mg/dL per g");
    Console.WriteLine($"Ritardo CHO: {choResults.DelayMean:F1} campioni ({choResults.DelayMean * 6:F0} minuti)");
    Console.WriteLine($"Qualità fit: R² = {choResults.AvgRSquared:F3}");
    Console.WriteLine($"Basato su {choResults.NumSegments} segmenti");
}

if (insulinResults != null)
{
    Console.WriteLine($"Costante di tempo Insulina: {insulinResults.TauMean:F0} ± {insulinResults.TauStd:F0} secondi");
    Console.WriteLine($"Guadagno Insulina: {insulinResults.GainMean:F2} ± {insulinResults.GainStd:F2} mg/dL per U");
    Console.WriteLine($"Ritardo Insulina: {insulinResults.DelayMean:F1} campioni ({insulinResults.DelayMean * 6:F0} minuti)");
    Console.WriteLine($"Qualità fit: R² = {insulinResults.AvgRSquared:F3}");
    Console.WriteLine($"Basato su {insulinResults.NumSegments} segmenti");
}
```

## Esempio: Ricerca Manuale di Segmenti Isolati

```csharp
// 1. Prepara i dati
var mealPoints = Identification.FromMeals(meals);
var injPoints = Identification.FromInjections(injections);
var glucosePoints = Identification.FromGlucoseRecords(glucose);

// 2. Trova segmenti isolati
var segments = Identification2.FindIsolatedSegments(
    mealPoints,
    injPoints,
    glucosePoints,
    isolationHours: 7.0,
    minDataHours: 4.0);

// 3. Analizza i segmenti trovati
var choSegments = segments.Where(s => s.InputIndex == 0).ToList();
var insulinSegments = segments.Where(s => s.InputIndex == 1).ToList();

Console.WriteLine($"Trovati {choSegments.Count} segmenti CHO isolati:");
foreach (var seg in choSegments)
{
    Console.WriteLine($"  - {seg.EventTime:dd/MM/yyyy HH:mm}: {seg.InputValue:F1}g CHO, " +
                      $"{seg.GlucoseData.Length} punti glucosio");
}

Console.WriteLine($"Trovati {insulinSegments.Count} segmenti Insulina isolati:");
foreach (var seg in insulinSegments)
{
    Console.WriteLine($"  - {seg.EventTime:dd/MM/yyyy HH:mm}: {seg.InputValue:F1}U insulina, " +
                      $"{seg.GlucoseData.Length} punti glucosio");
}
```

## Esempio: Identificazione di un Singolo Segmento

```csharp
// 1. Ottieni un segmento specifico
var segments = Identification2.FindIsolatedSegments(mealPoints, injPoints, glucosePoints);
var firstChoSegment = segments.FirstOrDefault(s => s.InputIndex == 0);

if (firstChoSegment != null)
{
    // 2. Identifica parametri per questo segmento
    var result = Identification2.IdentifySisoSegment(
        firstChoSegment,
        TsSeconds: 360.0,
        maxDelaySamples: 60,
        ridge: 0.01);

    if (result != null)
    {
        Console.WriteLine($"Evento: {result.EventTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Polo discreto a = {result.A:F4}");
        Console.WriteLine($"Guadagno discreto b = {result.B:F4}");
        Console.WriteLine($"Offset c = {result.C:F2} mg/dL");
        Console.WriteLine($"Ritardo d = {result.Delay} campioni ({result.Delay * 6} minuti)");
        Console.WriteLine($"---");
        Console.WriteLine($"Costante di tempo ? = {result.TimeConstant:F0} secondi ({result.TimeConstant / 3600:F1} ore)");
        Console.WriteLine($"Guadagno statico k = {result.StaticGain:F2}");
        Console.WriteLine($"---");
        Console.WriteLine($"Errore SSE = {result.SSE:F2}");
        Console.WriteLine($"RMSE = {result.RMSE:F2} mg/dL");
        Console.WriteLine($"R² = {result.RSquared:F3}");
    }
}
```

## Esempio: Identificazione con Parametri Personalizzati

```csharp
// Per pazienti con dinamiche più lente o dati più spaziati
var (choResults, insulinResults) = Identification2.IdentifyTwoChannels(
    mealPoints,
    injPoints,
    glucosePoints,
    TsSeconds: 360.0,           
    isolationHours: 10.0,       // Finestra più ampia (10 ore invece di 7)
    minDataHours: 6.0,          // Richiedi più dati (6 ore invece di 4)
    maxDelaySamples: 90,        // Ritardo massimo più lungo (9 ore)
    ridge: 0.05);               // Maggiore regolarizzazione

// Per pazienti pediatrici o con dinamiche più rapide
var (choResults2, insulinResults2) = Identification2.IdentifyTwoChannels(
    mealPoints,
    injPoints,
    glucosePoints,
    TsSeconds: 300.0,           // Campionamento 5 minuti
    isolationHours: 5.0,        // Finestra più stretta
    minDataHours: 3.0,          // Meno ore richieste
    maxDelaySamples: 36,        // Ritardo massimo 3 ore
    ridge: 0.01);
```

## Esempio: Aggregazione Personalizzata dei Risultati

```csharp
// 1. Identifica tutti i segmenti individualmente
var segments = Identification2.FindIsolatedSegments(mealPoints, injPoints, glucosePoints);
var choSegments = segments.Where(s => s.InputIndex == 0).ToList();

var individualResults = new List<Identification2.SisoResult>();
foreach (var seg in choSegments)
{
    var result = Identification2.IdentifySisoSegment(seg);
    if (result != null)
        individualResults.Add(result);
}

// 2. Filtra risultati con qualità bassa
var goodResults = individualResults
    .Where(r => r.RSquared > 0.7)           // Solo fit buoni
    .Where(r => r.TimeConstant < 14400)     // Tau < 4 ore
    .Where(r => r.TimeConstant > 1800)      // Tau > 30 minuti
    .Where(r => r.StaticGain > 0)           // Guadagno positivo per CHO
    .ToList();

// 3. Aggrega solo i risultati filtrati
var aggregated = Identification2.AggregateResults(goodResults, "CHO (filtrato)");

Console.WriteLine($"Risultati basati su {aggregated.NumSegments} segmenti di alta qualità:");
Console.WriteLine($"? = {aggregated.TauMean:F0} ± {aggregated.TauStd:F0} s");
Console.WriteLine($"k = {aggregated.GainMean:F2} ± {aggregated.GainStd:F2}");
```

## Esempio: Analisi della Variabilità Temporale

```csharp
// Identifica parametri su finestre temporali diverse per vedere variazioni
var windows = new[]
{
    (DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(-2), "Mese 1"),
    (DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(-1), "Mese 2"),
    (DateTime.Now.AddMonths(-1), DateTime.Now, "Mese 3")
};

foreach (var (start, end, label) in windows)
{
    var meals = blMeals.GetMeals(start, end);
    var injections = blInj.GetInjections(start, end);
    var glucose = blGlucose.GetSensorsRecords(start, end);

    var mealPts = Identification.FromMeals(meals);
    var injPts = Identification.FromInjections(injections);
    var glucosePts = Identification.FromGlucoseRecords(glucose);

    var (choRes, insRes) = Identification2.IdentifyTwoChannels(
        mealPts, injPts, glucosePts);

    Console.WriteLine($"\n{label}:");
    if (choRes != null)
        Console.WriteLine($"  CHO: ?={choRes.TauMean/3600:F1}h, k={choRes.GainMean:F2}, n={choRes.NumSegments}");
    if (insRes != null)
        Console.WriteLine($"  INS: ?={insRes.TauMean/3600:F1}h, k={insRes.GainMean:F2}, n={insRes.NumSegments}");
}
```

## Esempio: Export dei Risultati Individuali

```csharp
var (choResults, insulinResults) = Identification2.IdentifyTwoChannels(...);

// Export risultati CHO individuali
if (choResults != null)
{
    Console.WriteLine("EventTime,Tau[s],Gain,Delay,R2,RMSE");
    foreach (var res in choResults.IndividualResults)
    {
        Console.WriteLine($"{res.EventTime:yyyy-MM-dd HH:mm}," +
                         $"{res.TimeConstant:F0}," +
                         $"{res.StaticGain:F3}," +
                         $"{res.Delay}," +
                         $"{res.RSquared:F3}," +
                         $"{res.RMSE:F2}");
    }
}
```

## Esempio: Validazione Incrociata

```csharp
// Split dati in training e test
var allSegments = Identification2.FindIsolatedSegments(mealPoints, injPoints, glucosePoints);
var trainSegments = allSegments.Take(allSegments.Count * 2 / 3).ToList();
var testSegments = allSegments.Skip(allSegments.Count * 2 / 3).ToList();

// Identifica su training set
var trainResults = new List<Identification2.SisoResult>();
foreach (var seg in trainSegments)
{
    var res = Identification2.IdentifySisoSegment(seg);
    if (res != null) trainResults.Add(res);
}
var trainAgg = Identification2.AggregateResults(trainResults, "Training");

// Valida su test set usando parametri medi del training
Console.WriteLine($"Parametri da training: ?={trainAgg.TauMean:F0}s, k={trainAgg.GainMean:F2}");
Console.WriteLine($"Validazione su {testSegments.Count} segmenti di test:");

double sumTestError = 0;
foreach (var seg in testSegments)
{
    var testRes = Identification2.IdentifySisoSegment(seg);
    if (testRes != null)
    {
        double relErrorTau = Math.Abs(testRes.TimeConstant - trainAgg.TauMean) / trainAgg.TauMean;
        double relErrorK = Math.Abs(testRes.StaticGain - trainAgg.GainMean) / Math.Abs(trainAgg.GainMean);
        Console.WriteLine($"  {seg.EventTime:dd/MM}: ? err={relErrorTau:P0}, k err={relErrorK:P0}, R²={testRes.RSquared:F2}");
        sumTestError += testRes.RMSE;
    }
}
double avgTestRMSE = sumTestError / testSegments.Count;
Console.WriteLine($"RMSE medio su test set: {avgTestRMSE:F2} mg/dL");
```

## Note Importanti

### Thread Safety
Le funzioni di `Identification2` sono thread-safe e possono essere chiamate in parallelo su dataset diversi.

### Performance
- Il tempo di esecuzione dipende dal numero di segmenti trovati
- Ogni segmento richiede circa 50-200ms per l'identificazione
- Per 20 segmenti: tempo totale ~2-4 secondi

### Memoria
- Ogni `TimePoint[]` occupa circa 24 byte per punto
- Per 30 giorni a 6 minuti: ~7200 punti × 24 byte = ~170 KB
- I risultati aggregati occupano pochi KB

### Accuratezza Numerica
- Usa ridge regression per evitare matrici singolari
- Se `ridge = 0`, potrebbe sollevare eccezioni su dati patologici
- Valore raccomandato: `ridge = 0.01` per dati reali

### Gestione Errori
Tutte le funzioni restituiscono `null` se:
- Dati insufficienti
- Fit non riuscito
- Parametri fuori range fisico

Controllare sempre il valore di ritorno prima dell'uso!
