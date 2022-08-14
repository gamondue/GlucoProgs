using GlucoMan.BusinessLayer;
using static GlucoMan.Common;
using System;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace gamon
{
    internal partial class Accuracy
    {
        Entry txtQuantitative;
        Picker cmbQualitative;
        bool clickedFlag;
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
    }
}
