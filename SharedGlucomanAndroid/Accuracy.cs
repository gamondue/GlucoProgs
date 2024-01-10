using System;
using Xamarin.Forms;
using static GlucoMan.Common;
using Color = System.Drawing.Color;

namespace gamon
{
    internal class Accuracy
    {
        Entry txtQuantitative;
        Picker cmbQualitative;
        bool clickedFlag;

        internal QualitativeAccuracy GetQualitativeAccuracyGivenQuantitavive(double? NumericalAccuracy)
        {
            if (NumericalAccuracy < 0)
            {
                return QualitativeAccuracy.NotSet;
            }
            else if (NumericalAccuracy <= 5)
            {
                return QualitativeAccuracy.Null;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + 5)
            {
                return QualitativeAccuracy.AlmostNull;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + 5)
            {
                return QualitativeAccuracy.VeryBad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + 5)
            {
                return QualitativeAccuracy.Bad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + 5)
            {
                return QualitativeAccuracy.Poor;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + 5)
            {
                return QualitativeAccuracy.AlmostSufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + 5)
            {
                return QualitativeAccuracy.Sufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + 5)
            {
                return QualitativeAccuracy.Satisfactory;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                return QualitativeAccuracy.Good;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding + 5)
            {
                return QualitativeAccuracy.Outstanding;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + 5)
            {
                return QualitativeAccuracy.Perfect;
            }
            else
            {
                return QualitativeAccuracy.NotSet;
            }
        }
        internal Accuracy(Entry TextBox, Picker Combo)
        {
            txtQuantitative = TextBox;
            cmbQualitative = Combo;
            clickedFlag = false;
            //bl = Business;

            // hookup useful events 
            Combo.SelectedIndexChanged += Combo_SelectedIndexChanged;
            Combo.Unfocused += Combo_Unfocused;
            Combo.PropertyChanged += Combo_MouseClick;
            TextBox.TextChanged += TextBox_TextChanged;
        }
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
                if (Double.IsFinite(acc) && acc >= 0)
                {
                    cmbQualitative.SelectedItem =
                        GetQualitativeAccuracyGivenQuantitavive(acc);
                    txtQuantitative.BackgroundColor = AccuracyBackColor(acc);
                    txtQuantitative.TextColor = AccuracyForeColor(acc);
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
                    // the value (int) associated with the QualitativeAccuracy is given to the numerical accuracy
                    int accuracyNumber = (int)qa;
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
        internal Color AccuracyBackColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Color.Red;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull)
            {
                c = Color.DarkRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad)
            {
                c = Color.MediumVioletRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad)
            {
                c = Color.OrangeRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor)
            {
                c = Color.Orange;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient)
            {
                c = Color.Yellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient)
            {
                c = Color.YellowGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory)
            {
                c = Color.GreenYellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good)
            {
                c = Color.Lime;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding)
            {
                c = Color.DarkSeaGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect)
            {
                c = Color.Green;
            }
            else
            {
                c = Color.MintCream;
            }
            return c;
        }
        internal Color AccuracyForeColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect)
            {
                c = Color.White;
            }
            else
            {
                c = Color.White;
            }
            return c;
        }
    }
}
