using System;
using System.Collections.Generic;
using GlucoMan;

namespace GlucoMan
{
    class MainReadFreeStyleLibreData
    {
        private static string fileName;
        private static GlucoMan.ImportData import = new ImportData(); 

        static void Main(string[] args)
        {
            Console.Out.WriteLine("ReadFreeStyleLibreData");
            fileName = ".\\GabrieleMonti_glucose_4-7-2021.csv";
            List<GlucoseRecord> listGlucoseEvents = import.ImportDataFromFreeStyleLibre(fileName); 
            //inputContent.Count =
        }
    }
}
