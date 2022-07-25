using GlucoMan.BusinessLayer;
using GlucoMan.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
using static GlucoMan.Common;

namespace GlucoMan
{
    internal class Accuracy
    {
        TextBox txtQuantitative;
        ComboBox cmbQualitative; 
        bool clickedFlag;
        BL_MealAndFood bl = Common.MealAndFood_CommonBL;

        internal Accuracy (TextBox TextBox, ComboBox Combo, BL_MealAndFood Business)
        {
            txtQuantitative = TextBox;
            cmbQualitative = Combo;
            clickedFlag = false; 
            bl = Business; 

            // hookup useful events 
            Combo.SelectedIndexChanged += new System.EventHandler(Combo_SelectedIndexChanged);
            Combo.Leave += new System.EventHandler(Combo_Leave);
            Combo.MouseClick += new System.Windows.Forms.MouseEventHandler(Combo_MouseClick);
            TextBox.TextChanged += new System.EventHandler(TextBox_TextChanged);
        }
        internal double? QualitativeAccuracyChanged(QualitativeAccuracy QualitativeAccuracy)
        {
            // the value (int) associated with the QualitativeAccuracy is given to the numerical accuracy
            int accuracyNumber = (int)QualitativeAccuracy;
            bl.Meal.QualitativeAccuracyOfChoEstimate = QualitativeAccuracy;
            bl.Meal.AccuracyOfChoEstimate.Double = accuracyNumber;
            return bl.Meal.AccuracyOfChoEstimate.Double;
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            double acc;
            double.TryParse(txtQuantitative.Text, out acc);
            if (acc >= 0) //
            {
                cmbQualitative.SelectedItem =
                    bl.GetQualitativeAccuracyGivenQuantitavive(acc);
                txtQuantitative.BackColor = bl.AccuracyBackColor(acc);
                txtQuantitative.ForeColor = bl.AccuracyForeColor(acc);
            }
            else
            {
                cmbQualitative.SelectedItem = null;
                txtQuantitative.BackColor = Color.White;
                txtQuantitative.ForeColor = Color.Black;
            }
        }
        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clickedFlag)
            {
                clickedFlag = false;
                QualitativeAccuracy qa = (QualitativeAccuracy)(cmbQualitative.SelectedItem);
                txtQuantitative.Text = ((int)qa).ToString();
                QualitativeAccuracyChanged(qa);
                //callingForm.FromClassToUi();
            }
        }
        private void Combo_MouseClick(object sender, MouseEventArgs e)
        {
            clickedFlag = true;
        }
        private void Combo_Leave(object sender, EventArgs e)
        {
            clickedFlag = false;
        }
    }
}
