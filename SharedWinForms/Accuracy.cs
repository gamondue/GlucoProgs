using static GlucoMan.Common;

namespace gamon
{
    internal partial class Accuracy
    {
        TextBox txtQuantitative;
        ComboBox cmbQualitative;
        bool clickedFlag;

        internal QualitativeAccuracy GetQualitativeAccuracyGivenQuantitative(double? NumericalAccuracy)
        {
            if (NumericalAccuracy < 0)
            {
                return QualitativeAccuracy.NotSet;
            }
            else if (NumericalAccuracy == 0)
            {
                return QualitativeAccuracy.Null;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull)
            {
                return QualitativeAccuracy.AlmostNull;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad)
            {
                return QualitativeAccuracy.VeryBad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad)
            {
                return QualitativeAccuracy.Bad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor)
            {
                return QualitativeAccuracy.Poor;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient)
            {
                return QualitativeAccuracy.AlmostSufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient)
            {
                return QualitativeAccuracy.Sufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory)
            {
                return QualitativeAccuracy.Satisfactory;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good)
            {
                return QualitativeAccuracy.Good;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding)
            {
                return QualitativeAccuracy.Outstanding;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect)
            {
                return QualitativeAccuracy.Perfect;
            }
            else
            {
                return QualitativeAccuracy.NotSet;
            }
        }
        internal Accuracy(TextBox TextBox, ComboBox Combo)
        {
            txtQuantitative = TextBox;
            cmbQualitative = Combo;
            clickedFlag = false;

            // hookup useful events 
            Combo.SelectedIndexChanged += Combo_SelectedIndexChanged;
            Combo.Leave += Combo_Leave;
            Combo.MouseClick += Combo_MouseClick;
            TextBox.TextChanged += TextBox_TextChanged;
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            double acc;
            double.TryParse(txtQuantitative.Text, out acc);
            if (acc >= 0) //
            {
                cmbQualitative.SelectedItem =
                    GetQualitativeAccuracyGivenQuantitative(acc);
                txtQuantitative.BackColor = AccuracyBackColor(acc);
                txtQuantitative.ForeColor = AccuracyForeColor(acc);
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
                // the value (int) associated with the QualitativeAccuracy is given to the numerical accuracy
                //int accuracyNumber = (int)qa;
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
        internal Color AccuracyBackColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy < 0)
            {
                c = Color.Red;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + 5)
            {
                c = Color.DarkRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + 5)
            {
                c = Color.MediumVioletRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + 5)
            {
                c = Color.OrangeRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + 5)
            {
                c = Color.Orange;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + 5)
            {
                c = Color.Yellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + 5)
            {
                c = Color.YellowGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + 5)
            {
                c = Color.GreenYellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                c = Color.Lime;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding + 5)
            {
                c = Color.DarkSeaGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + 5)
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
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + 5)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + 5)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + 5)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + 5)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + 5)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + 5)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + 5)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding + 5)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + 5)
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
