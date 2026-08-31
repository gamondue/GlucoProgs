# GlucoMan

## Dati
Cambiare il fuso orario nei dati passati, quando sono stato in viaggio in fusi diversi (Turchia, Uzbekistan, ?? Bulgaria ??). Il campo UtcOffset deve essere aggiustato nelle tabelle: GlucoseRecords, Meals e SensorsRecords.

Analizzare le righe di dati del 2022 (e poche del 2024) che sono saltate fuori dall'ultima importazione da LibreView (tabelle Injections e Meals)

Per i grafici e le identificazioni il tempo preso deve essere riportato all'UTC del primo campionamento. 

# Database
Provare le operazioni su database per quel che riguarda la tabella SensorsRecords 

### Importazione da Freestyle libre
Indagare perché vengono creati degli strani record in date del 2022 nella tabella Injections, senza valore del bolo d'insulina e con tipo d'insulina 0.

Importare in SensorRecords anche il UtcOffset
Il dato non è presente fra i dati forniti da Abbot. 
Nel metodo:
internal override void InsertSensorMeasurements(List<GlucoseRecord> List)
Desumeremo l'UTC da quello del più vicino record della tabella Meals (è quella che viene aggiornata più accuratamente).

## Grafici
Aggiustare il grafico con dati sparsi che usa le curve di Bezier, evitando che i dati vadano nel giorno precedente. I grafici alla fine non sembrano corrispondere con i dati.

Il tempo dei grafici deve essere riportato all'UTC del primo campionamento

## Unità di misura
Permettere la cancellazione di una unità di misura (nei vari punti dove si usa oppure in una pagina specifica "Gestione delle Unità di Misura")

# FatSecret
Capire perché la ricerca con FatSecret dà così pochi risultati, quasi tutti in Inglese e non interessanti, rispetto all'uso dell'App, che ha un database di cibi italiani molto fornito.