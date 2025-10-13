using gamon; 

namespace GlucoMan.BusinessLayer
{
    public class BL_WeighFood
    {
        DataLayer dl = Common.Database;

        public DoubleAndText M0RawGross = new DoubleAndText();
        public DoubleAndText M0RawTare = new DoubleAndText();
        public DoubleAndText M0RawNet = new DoubleAndText();

        public DoubleAndText S1CookedGross = new DoubleAndText();
        public DoubleAndText S1CookedTare = new DoubleAndText();
        public DoubleAndText S1CookedNet = new DoubleAndText();

        public DoubleAndText T0RawGross = new DoubleAndText();
        public DoubleAndText T0RawTare = new DoubleAndText();
        public DoubleAndText T0RawNet = new DoubleAndText();
        //public  DoubleAndText T0CookedPlusTare = new DoubleAndText();

        public DoubleAndText S1pPotCookedPlusPot = new DoubleAndText(); // to remove !!
        //public  DoubleAndText DiDish = new DoubleAndText();
        //public  DoubleAndText T1CookedGross = new DoubleAndText();
        //public  DoubleAndText T1CookedTare = new DoubleAndText();
        //public  DoubleAndText T1CookedNet = new DoubleAndText();
        //public  DoubleAndText TpPortionWithAll = new DoubleAndText();
        //public  DoubleAndText M0pS1pPeRawFoodAndCooked = new DoubleAndText();
        //public  DoubleAndText M1MainfoodCooked = new DoubleAndText();
        //public  DoubleAndText M1pS1CourseCookedPlusCooked = new DoubleAndText();
        //public  DoubleAndText ACookRatio = new DoubleAndText();
        //public  DoubleAndText MppSpPortionOfCoursePlusCooked = new DoubleAndText();
        //public  DoubleAndText PPercPercentafPortion = new DoubleAndText();
        //public  DoubleAndText SpPortionOfCookedInGrams = new DoubleAndText();
        //public  DoubleAndText Mp1PortionCooked = new DoubleAndText();
        //public  DoubleAndText Mp0PortionReportedToRaw = new DoubleAndText(); 
        //public  DoubleAndText ChoCookedPercent = new DoubleAndText();
        //public  DoubleAndText ChoMainfoodPercent = new DoubleAndText();
        //public  DoubleAndText ChoTotalMainfood = new DoubleAndText();
        //public  DoubleAndText ChoTotalCooked = new DoubleAndText();

        public  void CalcUnknownData()
        {
            //T0RawGross.Double = T0RawGross.Double +
            //    S1CookedNet.Double + T0RawTare.Double;
            //M0pS1pPeRawFoodAndCooked.Double = T0RawGross.Double + S1CookedNet.Double;
            //M1MainfoodCooked.Double = T1Cooked.Double - S1CookedNet.Double - T0RawTare.Double;
            //M1pS1CourseCookedPlusCooked.Double = M1MainfoodCooked.Double + S1CookedNet.Double;
            //ACookRatio.Double = M1MainfoodCooked.Double / T0RawGross.Double;
            //MppSpPortionOfCoursePlusCooked.Double = Mp1PortionCooked.Double + SpPortionOfCookedInGrams.Double;
            //PPercpercentOfPortion.Double = (Mp1PortionCooked.Double + SpPortionOfCookedInGrams.Double) /
            //    (M1MainfoodCooked.Double + S1CookedNet.Double) * 100;
            //SpPortionOfCookedInGrams.Double = S1CookedNet.Double *
            //    PPercpercentOfPortion.Double / 100;
            //Mp0PortionReportedToRaw.Double = Mp1PortionCooked.Double / ACookRatio.Double;
            //Mp1PortionCooked.Double = TpPortionWithAll.Double -
            //    SpPortionOfCookedInGrams.Double - DiDish.Double;
            //ChoTotalMainfood.Double = Mp1PortionCooked.Double * ChoMainfoodPercent.Double / 100;
            //ChoTotalCooked.Double = SpPortionOfCookedInGrams.Double * ChoCookedPercent.Double / 100;

        }
        public  void SaveData()
        {
            //dl.SaveWeighFood(this);
        }
        public  void RestoreData()
        {
            //dl.RestoreWeighFood(this);
        }
    }
}
