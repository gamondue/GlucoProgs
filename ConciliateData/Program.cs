// See https://aka.ms/new-console-template for more information
using GlucoMan;
using System.Threading.Tasks;

Console.WriteLine("Concilate data from different sources");
FatSecret fs = new FatSecret();
await ProcessRepositories();


string freeStyleLibreFileName = @"C:\Users\gabri\OneDrive\Diabete\Dati\Freestyle Libre\Test.csv";
List<FreeStyleLibreRecord> listFreeStyleLibreEvents = FreeStyleLibre.ImportData(freeStyleLibreFileName);
Console.WriteLine("Time\tGlucose\tGluc.Hist.\tGluc. Scan");
foreach (FreeStyleLibreRecord gc in listFreeStyleLibreEvents)
{
    Console.WriteLine("{0},\t{1},\tGlucose Hist.{2},\tGlucose Scan: {3}",
        gc.Timestamp, gc.GlucoseValue, gc.GlucoseHistoricValue, gc.GlucoseScanValue, gc.CarbohydratesValue_g);
}

string DiabetesMFileName = @"C:\Users\gabri\OneDrive\Diabete\Dati\DiabetesM\Test.csv";
//string DiabetesMFileName = @"C:\Users\gabri\OneDrive\Diabete\Dati\DiabetesM\testFile.txt";
List<DiabetesMRecord> listDiabetesMEvents = DiabetesM.ImportData(DiabetesMFileName);
Console.WriteLine("Time\tGlucose\tGluc.Hist.\tGluc. Scan");
foreach (DiabetesMRecord gc in listDiabetesMEvents)
{
    Console.WriteLine("{0},\t{1},\tGlucose Hist.{2},\tGlucose Scan: {3}",
        gc.Timestamp, gc.GlucoseValue, gc.CarbohydratesValue_g);
}
Console.ReadLine();

static async Task ProcessRepositories()
{
}
