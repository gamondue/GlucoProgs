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
        frmMeals callingForm;
        TextBox txtQuantitative;
        ComboBox cmbQualitative; 
        bool clickedFlag;
        BL_MealAndFood bl;

        internal Accuracy (frmMeals CallingForm, TextBox TextBox, ComboBox Combo, BL_MealAndFood Business)
        {
            callingForm = CallingForm; 
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
        /// <summary>
        /// Changes qualitative accuracy when numerical accuracy changes
        /// </summary>
        /// <param name="currentMeal"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal QualitativeAccuracy NumericalAccuracyChanged(double? NumericalAccuracy)
        {
            if (NumericalAccuracy <= 0)
            {
                txtQuantitative.BackColor = Color.White;
                return QualitativeAccuracy.NotSet;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryBad)
            {
                txtQuantitative.BackColor = Color.Red;
                return QualitativeAccuracy.Null;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Bad)
            {
                txtQuantitative.BackColor = Color.DarkRed;
                return QualitativeAccuracy.VeryBad;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Poor)
            {
                txtQuantitative.BackColor = Color.OrangeRed;
                return QualitativeAccuracy.Bad;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.AlmostSufficient)
            {
                txtQuantitative.BackColor = Color.Orange;
                return QualitativeAccuracy.Poor;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Sufficient)
            {
                txtQuantitative.BackColor = Color.Yellow;
                return QualitativeAccuracy.AlmostSufficient;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Satisfactory)
            {
                txtQuantitative.BackColor = Color.GreenYellow;
                return QualitativeAccuracy.Sufficient;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Good)
            {
                txtQuantitative.BackColor = Color.ForestGreen;
                return QualitativeAccuracy.Satisfactory;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryGood)
            {
                txtQuantitative.BackColor = Color.LawnGreen;
                return QualitativeAccuracy.Good;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Outstanding)
            {
                txtQuantitative.BackColor = Color.DarkSeaGreen;
                return QualitativeAccuracy.VeryGood;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Perfect)
            {
                txtQuantitative.BackColor = Color.DarkOliveGreen;
                txtQuantitative.ForeColor = Color.White;
                return QualitativeAccuracy.Outstanding;
            }
            else
            {
                txtQuantitative.BackColor = Color.Green;
                return QualitativeAccuracy.Perfect;
            }
        }
        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clickedFlag)
            {
                clickedFlag = false;
                QualitativeAccuracy qa = (QualitativeAccuracy)(cmbQualitative.SelectedItem);
                QualitativeAccuracyChanged(qa);
                callingForm.FromClassToUi();
            }
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            bl.Meal.AccuracyOfChoEstimate.Double = Safe.Double(txtQuantitative.Text);
            NumericalAccuracyChanged(bl.Meal.AccuracyOfChoEstimate.Double);
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
