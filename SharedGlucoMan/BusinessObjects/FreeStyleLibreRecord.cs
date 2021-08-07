using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    class FreeStyleLibreRecord : GlucoseRecord
    {
        int? typeOfDocument;
        double? glucoseHistoricValue;
        double? glucoseScanValue;
        string insulinRapidActionString;
        double? insulinRapidActionValue;
        string insulinSlowActionString;
        double? insulinSlowActionValue;
        double? glucoseStripValue_mg_dL;
        double? chetons_mmol_L; 

        public int? TypeOfDocument { get => typeOfDocument; set => typeOfDocument = value; }
        public double? GlucoseHistoricValue { get => glucoseHistoricValue; set => glucoseHistoricValue = value; }
        public double? GlucoseScanValue { get => glucoseScanValue; set => glucoseScanValue = value; }
        public string InsulinRapidActionString { get => insulinRapidActionString; set => insulinRapidActionString = value; }
        public double? InsulinRapidActionValue { get => insulinRapidActionValue; set => insulinRapidActionValue = value; }
        public string InsulinSlowActionString { get => insulinSlowActionString; set => insulinSlowActionString = value; }
        public double? InsulinSlowActionValue { get => insulinSlowActionValue; set => insulinSlowActionValue = value; }
        public double? GlucoseStripValue_mg_dL { get => glucoseStripValue_mg_dL; set => glucoseStripValue_mg_dL = value; }
        public double? Chetons_mmol_L { get => chetons_mmol_L; set => chetons_mmol_L = value; }
        public double? MealInsulin { get; internal set; }
        public double? InsulinCorrection { get; internal set; }
        public double? InsulinWithUsersModifications { get; internal set; }
    }
}
