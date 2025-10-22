using gamon;

namespace GlucoMan.BusinessLayer
{
    public class BL_WeighFood
    {
        DataLayer dl = Common.Database;

        public DoubleAndText RawGross = new DoubleAndText();
        public DoubleAndText RawTare = new DoubleAndText();
        public DoubleAndText RawNet = new DoubleAndText();

        public DoubleAndText CookedGross = new DoubleAndText();
        public DoubleAndText CookedTare = new DoubleAndText();
        public DoubleAndText CookedNet = new DoubleAndText();

        public DoubleAndText CookedPortionGross = new DoubleAndText();
        public DoubleAndText CookedPortionTare = new DoubleAndText();
        public DoubleAndText CookedPortionNet = new DoubleAndText();

        public DoubleAndText NPortions = new DoubleAndText();

        public void CalcUnknownData()
        {
            // Placeholder for future calculations
            // Currently not implementing complex weight calculations
        }

        /// <summary>
        /// Saves weighing data to Parameters table (first row, without timestamp)
        /// </summary>
        public void SaveData()
        {
            try
            {
                // Raw food weighing data
                dl.SaveParameter("Weigh_RawGross", RawGross.Text);
                dl.SaveParameter("Weigh_RawTare", RawTare.Text);
                dl.SaveParameter("Weigh_RawNet", RawNet.Text);

                // Cooked food weighing data
                dl.SaveParameter("Weigh_CookedGross", CookedGross.Text);
                dl.SaveParameter("Weigh_CookedTare", CookedTare.Text);
                dl.SaveParameter("Weigh_CookedNet", CookedNet.Text);

                // Cooked portion weighing data
                dl.SaveParameter("Weigh_CookedPortionGross", CookedPortionGross.Text);
                dl.SaveParameter("Weigh_CookedPortionTare", CookedPortionTare.Text);
                dl.SaveParameter("Weigh_CookedPortionNet", CookedPortionNet.Text);

                // Number of portions
                dl.SaveParameter("Weigh_NPortions", NPortions.Text);

                General.LogOfProgram?.Event("BL_WeighFood - Weighing data saved successfully");
            }
            catch (System.Exception ex)
            {
                General.LogOfProgram?.Error("BL_WeighFood - SaveData", ex);
            }
        }

        /// <summary>
        /// Restores weighing data from Parameters table (first row)
        /// </summary>
        public void RestoreData()
        {
            try
            {
                // Raw food weighing data
                RawGross.Text = dl.RestoreParameter("Weigh_RawGross") ?? "";
                RawTare.Text = dl.RestoreParameter("Weigh_RawTare") ?? "";
                RawNet.Text = dl.RestoreParameter("Weigh_RawNet") ?? "";

                // Cooked food weighing data
                CookedGross.Text = dl.RestoreParameter("Weigh_CookedGross") ?? "";
                CookedTare.Text = dl.RestoreParameter("Weigh_CookedTare") ?? "";
                CookedNet.Text = dl.RestoreParameter("Weigh_CookedNet") ?? "";

                // Cooked portion weighing data
                CookedPortionGross.Text = dl.RestoreParameter("Weigh_CookedPortionGross") ?? "";
                CookedPortionTare.Text = dl.RestoreParameter("Weigh_CookedPortionTare") ?? "";
                CookedPortionNet.Text = dl.RestoreParameter("Weigh_CookedPortionNet") ?? "";

                // Number of portions
                NPortions.Text = dl.RestoreParameter("Weigh_NPortions") ?? "";

                General.LogOfProgram?.Event("BL_WeighFood - Weighing data restored successfully");
            }
            catch (System.Exception ex)
            {
                General.LogOfProgram?.Error("BL_WeighFood - RestoreData", ex);
            }
        }
    }
}
