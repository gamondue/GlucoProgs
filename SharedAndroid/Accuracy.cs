using GlucoMan.BusinessLayer;
using GlucoMan.Mobile;
using System;
using Xamarin.Forms;
using static GlucoMan.Common;
using Color = System.Drawing.Color;

namespace GlucoMan
{
    internal class Accuracy
    {
        Entry txtQuantitative;
        Picker cmbQualitative;
        bool clickedFlag;
        BL_MealAndFood bl;
        internal Accuracy(Entry TextBox, Picker Combo, BL_MealAndFood Business)
        {
            txtQuantitative = TextBox;
            cmbQualitative = Combo;
            clickedFlag = false;
            bl = Business;

            // hookup useful events 
            Combo.SelectedIndexChanged += new System.EventHandler(Combo_SelectedIndexChanged);
            Combo.Unfocused += Combo_Unfocused;
            Combo.PropertyChanged += Combo_MouseClick;
            TextBox.TextChanged += TextBox_TextChanged;
        }
        internal double? QualitativeAccuracyChanged(QualitativeAccuracy QualitativeAccuracy)
        {
            // the value (int) associated with the QualitativeAccuracy is given to the numerical accuracy
            int accuracyNumber = (int)QualitativeAccuracy;
            bl.Meal.QualitativeAccuracyOfChoEstimate = QualitativeAccuracy;
            bl.Meal.AccuracyOfChoEstimate.Double = accuracyNumber;
            return bl.Meal.AccuracyOfChoEstimate.Double;
        }
        ///// <summary>
        ///// Changes qualitative accuracy when numerical accuracy changes
        ///// </summary>
        ///// <param name="currentMeal"></param>
        ///// <exception cref="NotImplementedException"></exception>
        //internal QualitativeAccuracy NumericalAccuracyChanged(double? NumericalAccuracy)
        //{
        //    if (NumericalAccuracy <= 0)
        //    {
        //        txtQuantitative.BackgroundColor = Color.White;
        //        return QualitativeAccuracy.NotSet;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryBad)
        //    {
        //        txtQuantitative.BackgroundColor = Color.Red;
        //        return QualitativeAccuracy.Null;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.Bad)
        //    {
        //        txtQuantitative.BackgroundColor = Color.DarkRed;
        //        return QualitativeAccuracy.VeryBad;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.Poor)
        //    {
        //        txtQuantitative.BackgroundColor = Color.OrangeRed;
        //        return QualitativeAccuracy.Bad;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.AlmostSufficient)
        //    {
        //        txtQuantitative.BackgroundColor = Color.Orange;
        //        return QualitativeAccuracy.Poor;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.Sufficient)
        //    {
        //        txtQuantitative.BackgroundColor = Color.Yellow;
        //        return QualitativeAccuracy.AlmostSufficient;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.Satisfactory)
        //    {
        //        txtQuantitative.BackgroundColor = Color.GreenYellow;
        //        return QualitativeAccuracy.Sufficient;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.Good)
        //    {
        //        txtQuantitative.BackgroundColor = Color.ForestGreen;
        //        return QualitativeAccuracy.Satisfactory;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryGood)
        //    {
        //        txtQuantitative.BackgroundColor = Color.LawnGreen;
        //        return QualitativeAccuracy.Good;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.Outstanding)
        //    {
        //        txtQuantitative.BackgroundColor = Color.DarkSeaGreen;
        //        return QualitativeAccuracy.VeryGood;
        //    }
        //    else if (NumericalAccuracy < (double)QualitativeAccuracy.Perfect)
        //    {
        //        txtQuantitative.BackgroundColor = Color.DarkOliveGreen;
        //        return QualitativeAccuracy.Outstanding;
        //    }
        //    else
        //    {
        //        txtQuantitative.BackgroundColor = Color.Green;
        //        return QualitativeAccuracy.Perfect;
        //    }
        //}
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double acc;
            if (!double.TryParse(txtQuantitative.Text, out acc))
            {
                cmbQualitative.SelectedItem = null;
                txtQuantitative.Text = "";
                txtQuantitative.BackgroundColor = Color.White;
                txtQuantitative.TextColor = Color.Black;
            }
            else
            {
                if (Double.IsFinite(acc) && acc != 0)
                {
                    cmbQualitative.SelectedItem =
                        bl.GetQualitativeAccuracyGivenQuantitavive(acc);
                    txtQuantitative.BackgroundColor = bl.AccuracyBackColor(acc);
                    txtQuantitative.TextColor = bl.AccuracyForeColor(acc);
                }
                else
                {
                    cmbQualitative.SelectedItem = null;
                    txtQuantitative.Text = "";
                    txtQuantitative.BackgroundColor = Color.White;
                    txtQuantitative.TextColor = Color.Black;
                }
            }
            //NumericalAccuracyChanged(bl.Meal.AccuracyOfChoEstimate.Double);
        }
        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clickedFlag)
            {
                clickedFlag = false;
                if (cmbQualitative.SelectedItem != null)
                {
                    QualitativeAccuracy qa = (QualitativeAccuracy)(cmbQualitative.SelectedItem);
                    txtQuantitative.Text = ((int)qa).ToString();
                    QualitativeAccuracyChanged(qa);
                }
            }
        }
        private void Combo_MouseClick(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            clickedFlag = true;
        }
        private void Combo_Unfocused(object sender, FocusEventArgs e)
        { 
            clickedFlag = false;
        }
    }
}
