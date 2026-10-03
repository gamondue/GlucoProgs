# GlucoMan

## Dati
- Cambiare il fuso orario nei dati passati (già prodotti nel passato), quando sono stato in viaggio in fusi diversi (Turchia, Uzbekistan, ?? Bulgaria ??, Portogallo). Il campo UtcOffset deve essere aggiustato nelle tabelle: GlucoseRecords, Meals e SensorsRecords.

- Analizzare le righe di dati del 2022 (e poche del 2024) che sono saltate fuori dall'ultima importazione da LibreView (tabelle Injections e Meals)

# Database
- Provare le operazioni su database per quel che riguarda la tabella SensorsRecords 

### Importazione da Freestyle libre
- Indagare perché vengono creati degli strani record in date del 2022 nella tabella Injections, senza valore del bolo d'insulina e con tipo d'insulina 0.

- Fatto, DA VERIFICARE. Importare in SensorRecords anche il UtcOffset. Il dato non è presente fra i dati forniti da Abbot, per cui nel metodo:
internal override void InsertSensorMeasurements(List<GlucoseRecord> List)
Desumeremo l'UTC da quello del più vicino record presente fra le tabelle: Meals, GlucoseRecords e Injections.

## MealPage
In Android gli entry txtFoodCarbohydratesPerUnit, txtFoodQuantityInUnits e txtFoodCarbohydratesGrams non ammette la virgola quando non è "attaccato" ad una riga della CollectionView sottostante (quando nelle entry si sta immettendo un nuovo cibo). Dopo che il cibo è stato fatto passare fra le righe della CollectionView (con tasto +), si possono mettere cifre dopo la virgola. In Windows questo non succede.


## FoodPage
Quando si aggiunge una unità di misura, con il bottone btnAddUnit, aggiungiamo alla "MessageBox" "Applicabilità della unità" l'opzione "Annulla" che non deve creare nessuna unità di misura.  
Se se l'utente scegli l'applicabilità per tutti bisogna chiedere conferma con un prompt "Attenzione se si conferma questa unità diverrà applicabile ad ogni cibo", opzioni: "Conferma" e "Non conferma".
Se l'utente sceglie "Non conferma" si deve riproporre la "MessageBox" "Applicabilità della unità".

## Grafici
- Fatto, DA VERIFICARE. Per i grafici e le identificazioni il tempo preso deve essere riportato all'UTC del primo campionamento. Quando cambia l'ora legale che deve essere un salto nei grafici o due grafici nello stesso periodo. Il tempo dei grafici deve essere riportato all'UTC del primo campionamento (cambiato idea, fatto che si riporta all'ora attulmente configurata).

## Unità di misura
- FATTO e verificato. Permettere la cancellazione di una unità di misura (nella pagina dei dettagli su un cibo)

# FatSecret
- Capire perché la ricerca con FatSecret dà così pochi risultati, quasi tutti in Inglese e non interessanti, rispetto all'uso dell'App, che ha un database di cibi italiani molto fornito.