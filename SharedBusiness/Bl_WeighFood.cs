using gamon;

namespace GlucoMan
{
    public class BL_WeighFood
    {
        public WeighingData Data { get; set; } = new WeighingData();

        DataLayer dl = DatabaseService.Instance.Database;

        internal enum  TypeOfWeigh
        {
            Gross,
            Tare,
            Net
        }

        /// <summary>
        /// Calculates Summary Data: Raw/CookedFood ratio, Weight of portion, CHO of portion
        /// Called whenever any weight or portion value changes
        /// </summary>
        internal void CalculateSummaryData()
        {
            try
            {
                //General.LogOfProgram?.Debug($"BL_WeighFood - CalculateSummaryData STARTED");
                //General.LogOfProgram?.Debug($"BL_WeighFood - Parsed values: RawNet={Data.Raw.Net.Double}, CookedNet={Data.CookedFood.Net.Double}, PortionNet={Data.Portion.Net.Double}, NPortions={Data.NPortions.Int}, CarbohydratesPercent={Data.CarbohydratesPercent.Double}");

                // temporary value of total cooked
                double? totalCookedWeightTemp = Data.CookedFood.Net.Double;

                // if total cooked food is "null" we put the raw net as weight portion
                // (will be overwritten by the next part if the rest is not null)
                if (ValueIsNullOrZero(Data.CookedFood.Net))
                    totalCookedWeightTemp = Data.Raw.Net.Double;

                // if we are weighing the portion and portion net value is null,
                // we put the raw net as weight portion
                if (ValueIsNullOrZero(Data.CookedFood.Net) && Data.DoWeighCookedPortion)
                    totalCookedWeightTemp = Data.CookedFood.Net.Double;

                if (ValueIsNullOrZero(Data.CookedSeasoning.Net))
                {
                    //  if we have no seasoning we don't need to measure CHO
                    Data.TotalCarbohydratesPercent.Double = Data.FoodCarbohydratesPercent.Double;
                }
                else
                {
                    // totalCookedWeightTemp contains the weiight of the seasoning

                    // the CHO of all is the weighted mean of foood and seasonin
                    Data.TotalCarbohydratesPercent.Double =
                        ((Data.CookedFood.Net.Double - Data.CookedSeasoning.Net.Double) * Data.FoodCarbohydratesPercent.Double +
                        Data.CookedSeasoning.Net.Double * Data.CookedSeasoning.CarbohydratesPercent.Double)
                            / totalCookedWeightTemp;
                }

                Data.RawCookedRatio.Double = Data.Raw.Net.Double / totalCookedWeightTemp;

                Data.WeightOfPortion.Double = 0;
                Data.CarbohydratesOfPortion.Double = 0;

                // Calculate based on selected option
                if (Data.DoWeighCookedPortion)
                {
                    // Weigh the portion option
                    if (Data.IsChoOfRawFood)
                    {
                        // CHO of raw food enabled: Weight of portion = Raw/cooked ratio * Net of portion
                        Data.WeightOfPortion.Double = Data.RawCookedRatio.Double * Data.Portion.Net.Double;
                    }
                    else
                    {
                        // CHO of raw food disabled: Weight of portion = Net of portion
                        Data.WeightOfPortion.Double = Data.Portion.Net.Double;
                    }
                }
                else if (Data.NPortions.Int > 0)
                {
                    // Equal portions option
                    if (Data.IsChoOfRawFood)
                    {
                        Data.WeightOfPortion.Double = Data.RawCookedRatio.Double * totalCookedWeightTemp
                            / Data.NPortions.Int;
                    }
                    else
                    {
                        Data.WeightOfPortion.Double = totalCookedWeightTemp / Data.NPortions.Int;
                    }
                }
                // CHO [g] of portion = Weight of portion * CHO [%] / 100
                Data.CarbohydratesOfPortion.Double = Data.WeightOfPortion.Double * Data.TotalCarbohydratesPercent.Double / 100;

                General.LogOfProgram?.Event($"BL_WeighFood - Summary calculated: Ratio={Data.RawCookedRatio.Double:F3}, Weight={Data.WeightOfPortion.Double:F1}g, CHO={Data.CarbohydratesOfPortion.Double:F1}g");

                // Save weighing Data to database after calculation
                General.LogOfProgram?.Debug($"BL_WeighFood - About to call SaveData()");
                SaveData();
                General.LogOfProgram?.Debug($"BL_WeighFood - CalculateSummaryData COMPLETED");
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_WeighFood - CalculateSummaryData", ex);
            }
        }

        /// <summary>
        /// Saves weighing Data to Parameters table (first row, without timestamp)
        /// </summary>
        internal void SaveData()
        {
            try
            {
                // Food identification and CHO Data
                dl.SaveParameter("Weigh_FoodId", Data.FoodId ?? "");
                dl.SaveParameter("Weigh_FoodName", Data.FoodName ?? "");
                dl.SaveParameter("Weigh_FoodCarbohydratesPercent", Data.FoodCarbohydratesPercent.Text);
                dl.SaveParameter("Weigh_TotalCarbohydratesPercent", Data.TotalCarbohydratesPercent.Text);

                // Raw food weighing Data
                dl.SaveParameter("Weigh_RawGross", Data.Raw.Gross.Text);
                dl.SaveParameter("Weigh_RawTare", Data.Raw.Tare.Text);
                dl.SaveParameter("Weigh_RawNet", Data.Raw.Net.Text);

                // CookedFood food weighing Data
                dl.SaveParameter("Weigh_CookedGross", Data.CookedFood.Gross.Text);
                dl.SaveParameter("Weigh_CookedTare", Data.CookedFood.Tare.Text);
                dl.SaveParameter("Weigh_CookedNet", Data.CookedFood.Net.Text);

                // CookedSeasoning weighing Data
                dl.SaveParameter("Weigh_SeasoningGross", Data.CookedSeasoning.Gross.Text);
                dl.SaveParameter("Weigh_SeasoningTare", Data.CookedSeasoning.Tare.Text);
                dl.SaveParameter("Weigh_SeasoningNet", Data.CookedSeasoning.Net.Text);
                dl.SaveParameter("Weigh_SeasoningCarbohydratesPercent", Data.CookedSeasoning.CarbohydratesPercent.Text);

                // Portion weighing Data
                dl.SaveParameter("Weigh_PortionGross", Data.Portion.Gross.Text);
                dl.SaveParameter("Weigh_PortionTare", Data.Portion.Tare.Text);
                dl.SaveParameter("Weigh_PortionNet", Data.Portion.Net.Text);

                // Number of portions and options
                dl.SaveParameter("Weigh_NPortions", Data.NPortions.Text);
                dl.SaveParameter("Weigh_DoWeighCookedPortion", Data.DoWeighCookedPortion.ToString());
                dl.SaveParameter("Weigh_IsChoOfRawFood", Data.IsChoOfRawFood.ToString());

                // Calculated values
                dl.SaveParameter("Weigh_RawCookedRatio", Data.RawCookedRatio.Text);
                dl.SaveParameter("Weigh_WeightOfPortion", Data.WeightOfPortion.Text);
                dl.SaveParameter("Weigh_CarbohydratesOfPortion", Data.CarbohydratesOfPortion.Text);

                General.LogOfProgram?.Event("BL_WeighFood - Weighing data saved successfully");
            }
            catch (System.Exception ex)
            {
                General.LogOfProgram?.Error("BL_WeighFood - SaveData", ex);
            }
        }

        /// <summary>
        /// Restores weighing Data from Parameters table (first row)
        /// </summary>
        internal void RestoreData()
        {
            try
            {
                // Food identification and CHO Data
                Data.FoodId = dl.RestoreParameter("Weigh_FoodId") ?? "";
                Data.FoodName = dl.RestoreParameter("Weigh_FoodName") ?? "";
                Data.FoodCarbohydratesPercent.Text = dl.RestoreParameter("Weigh_FoodCarbohydratesPercent") ?? "";
                Data.TotalCarbohydratesPercent.Text = dl.RestoreParameter("Weigh_TotalCarbohydratesPercent") ?? "";

                // Raw food weighing Data
                Data.Raw.Gross.Text = dl.RestoreParameter("Weigh_RawGross") ?? "";
                Data.Raw.Tare.Text = dl.RestoreParameter("Weigh_RawTare") ?? "";
                Data.Raw.Net.Text = dl.RestoreParameter("Weigh_RawNet") ?? "";

                // CookedFood food weighing Data
                Data.CookedFood.Gross.Text = dl.RestoreParameter("Weigh_CookedGross") ?? "";
                Data.CookedFood.Tare.Text = dl.RestoreParameter("Weigh_CookedTare") ?? "";
                Data.CookedFood.Net.Text = dl.RestoreParameter("Weigh_CookedNet") ?? "";

                // CookedSeasoning weighing Data
                
                Data.CookedSeasoning.Gross.Text = dl.RestoreParameter("Weigh_SeasoningGross") ?? "";
                Data.CookedSeasoning.Tare.Text = dl.RestoreParameter("Weigh_SeasoningTare") ?? "";
                Data.CookedSeasoning.Net.Text = dl.RestoreParameter("Weigh_SeasoningNet") ?? "";
                Data.CookedSeasoning.CarbohydratesPercent.Text = dl.RestoreParameter("Weigh_SeasoningCarbohydratesPercent") ?? "";

                // Portion weighing Data
                Data.Portion.Gross.Text = dl.RestoreParameter("Weigh_PortionGross") ?? "";
                Data.Portion.Tare.Text = dl.RestoreParameter("Weigh_PortionTare") ?? "";
                Data.Portion.Net.Text = dl.RestoreParameter("Weigh_PortionNet") ?? "";

                // Number of portions and options
                Data.NPortions.Text = dl.RestoreParameter("Weigh_NPortions") ?? "";
                
                string doWeighCookedPortionStr = dl.RestoreParameter("Weigh_DoWeighCookedPortion") ?? "False";
                Data.DoWeighCookedPortion = bool.TryParse(doWeighCookedPortionStr, out bool doWeighCookedPortion) && doWeighCookedPortion;
                
                string isChoOfRawFoodStr = dl.RestoreParameter("Weigh_IsChoOfRawFood") ?? "False";
                Data.IsChoOfRawFood = bool.TryParse(isChoOfRawFoodStr, out bool isChoOfRawFood) && isChoOfRawFood;

                // Calculated values
                Data.RawCookedRatio.Text = dl.RestoreParameter("Weigh_RawCookedRatio") ?? "";
                Data.WeightOfPortion.Text = dl.RestoreParameter("Weigh_WeightOfPortion") ?? "";
                Data.CarbohydratesOfPortion.Text = dl.RestoreParameter("Weigh_CarbohydratesOfPortion") ?? "";

                General.LogOfProgram?.Event("BL_WeighFood - Weighing data restored successfully");
            }
            catch (System.Exception ex)
            {
                General.LogOfProgram?.Error("BL_WeighFood - RestoreData", ex);
            }
        }
        internal void CalculateThirdFromTwoAndSummaryData(WeightsForWeighing Weights, TypeOfWeigh TypeOfModifiedWeight)
        {
            if (Weights == null)
                return;

            switch (TypeOfModifiedWeight)
            {
                case TypeOfWeigh.Gross:
                    {
                        if (ValueIsNullOrZero(Weights.Gross))
                            // if the passed value is "null", do nothing
                            return;
                        // gross not null, lets check the tare
                        if (ValueIsNullOrZero(Weights.Tare))
                            // gross not null and tare is null, lets check the net
                            if (ValueIsNullOrZero(Weights.Net))
                                // also the net is null we do not have enough values to calculate anything
                                // so we do nothing but contunue the calculations
                                break;
                            else
                                // the net has a value, we the tare calculate it with the other two
                                Weights.Tare.Double = Weights.Gross.Double - Weights.Net.Double;
                        else
                            // if the tare has a value, we calculate the net
                            Weights.Net.Double = Weights.Gross.Double - Weights.Tare.Double;
                        break;
                    }
                case TypeOfWeigh.Tare:
                    { 
                        //if (ValueIsNullOrZero(Weights.Tare))
                        //    // if the passed value is "null", do nothing
                        //    return;
                        // tare not null, lets check the gross
                        if (ValueIsNullOrZero(Weights.Gross))
                            // tare not null and gross null, lets check the net
                            if (ValueIsNullOrZero(Weights.Net))
                                // if also the net is null we do not have enough values to calculate anything
                                // so we do nothing but contunue the calculations
                                break;
                            else
                                // if the tare has not a value we calculate it with the other two
                                Weights.Tare.Double = Weights.Gross.Double - Weights.Net.Double;
                        else
                            // if the tare and the gross have a value, we calculate the net
                             Weights.Net.Double = Weights.Gross.Double - Weights.Tare.Double;
                        break;
                    }
                case TypeOfWeigh.Net:
                    {
                        if (ValueIsNullOrZero(Weights.Net))
                            // if the passed value is "null", do nothing
                            return;
                        // net not null
                        if (ValueIsNullOrZero(Weights.Gross))
                            // net not null and gross is null
                            if (ValueIsNullOrZero(Weights.Tare))
                                // if also the net is null we do not have enough values to calculate anything
                                // so we do nothing but we keep calculating
                                break;
                            else
                                // gross and net have a value, so we calculate the tare with the other two
                                Weights.Tare.Double = Weights.Gross.Double - Weights.Net.Double;
                        else
                            // net and the gross have a value, we calculate the taare with these values
                            Weights.Tare.Double = Weights.Gross.Double - Weights.Net.Double;
                        break;
                    }
            }
            CalculateSummaryData();
        }
        private bool ValueIsNullOrZero(DoubleAndText ObjectValue)
        {
            return (ObjectValue == null || ObjectValue.Double == null || ObjectValue.Double <= 0);
        }
    }
}
