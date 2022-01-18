using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class WeighData
    {
        public DoubleAndText M0RawGross = new DoubleAndText();
        public DoubleAndText M0RawTare = new DoubleAndText();
        public DoubleAndText M0RawNet = new DoubleAndText();

        public DoubleAndText S1SauceGross = new DoubleAndText();
        public DoubleAndText S1SauceTare = new DoubleAndText();
        public DoubleAndText S1SauceNet = new DoubleAndText();

        public DoubleAndText T0RawGross = new DoubleAndText();
        public DoubleAndText T0RawTare = new DoubleAndText();
        public DoubleAndText T0RawNet = new DoubleAndText();
        public DoubleAndText T0SaucePlusTare = new DoubleAndText();

        public DoubleAndText S1pPotSaucePlusPot = new DoubleAndText(); // eliminare !!
        public DoubleAndText DiDish = new DoubleAndText();
        public DoubleAndText T1CookedGross = new DoubleAndText();
        public DoubleAndText T1CookedTare = new DoubleAndText();
        public DoubleAndText T1CookedNet = new DoubleAndText();
        public DoubleAndText TpPortionWithAll = new DoubleAndText();
        public DoubleAndText M0pS1pPeRawFoodAndSauce = new DoubleAndText();
        public DoubleAndText M1MainfoodCooked = new DoubleAndText();
        public DoubleAndText M1pS1CourseCookedPlusSauce = new DoubleAndText();
        public DoubleAndText ACookRatio = new DoubleAndText();
        public DoubleAndText MppSpPortionOfCoursePlusSauce = new DoubleAndText();
        public DoubleAndText PPercPercentOfPortion = new DoubleAndText();
        public DoubleAndText SpPortionOfSauceInGrams = new DoubleAndText();
        public DoubleAndText Mp1PortionCooked = new DoubleAndText();
        public DoubleAndText Mp0PortionReportedToRaw = new DoubleAndText();
        public DoubleAndText ChoSaucePercent = new DoubleAndText();
        public DoubleAndText ChoMainfoodPercent = new DoubleAndText();
        public DoubleAndText ChoTotalMainfood = new DoubleAndText();
        public DoubleAndText ChoTotalSauce = new DoubleAndText();

    }
}
