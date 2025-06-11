using static GlucoMan.Common;

namespace gamon
{
    internal class UiAccuracy
    {
        private Entry txtQuantitative;
        private Picker cmbQualitative;
        private int halfInterval = ((int)QualitativeAccuracy.Perfect - (int)QualitativeAccuracy.Null) / 20;
        private bool editingNumericAccuracy = false;

        internal QualitativeAccuracy GetQualitativeAccuracyGivenQuantitavive(double? NumericalAccuracy)
        {
            if (NumericalAccuracy < 0 || NumericalAccuracy > 100)
            {
                return QualitativeAccuracy.NotSet;
            }
            else if (NumericalAccuracy <= halfInterval)
            {
                return QualitativeAccuracy.Null;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + halfInterval)
            {
                return QualitativeAccuracy.AlmostNull;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + halfInterval)
            {
                return QualitativeAccuracy.VeryBad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + halfInterval)
            {
                return QualitativeAccuracy.Bad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + halfInterval)
            {
                return QualitativeAccuracy.Poor;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + halfInterval)
            {
                return QualitativeAccuracy.AlmostSufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + halfInterval)
            {
                return QualitativeAccuracy.Sufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + halfInterval)
            {
                return QualitativeAccuracy.Satisfactory;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + halfInterval)
            {
                return QualitativeAccuracy.Good;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding + halfInterval)
            {
                return QualitativeAccuracy.Outstanding;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + halfInterval)
            {
                return QualitativeAccuracy.Perfect;
            }
            else
            {
                return QualitativeAccuracy.NotSet;
            }
        }
        internal UiAccuracy(Entry TextBox, Picker Combo)
        {
            txtQuantitative = TextBox;
            cmbQualitative = Combo;

            // hookup useful events 
            Combo.SelectedIndexChanged += Combo_SelectedIndexChanged;
            //Combo.Unfocused += Combo_Unfocused;
            //Combo.PropertyChanged += Combo_MouseClick;
            //TextBox.TextChanged += TextBox_Unfocused;
            TextBox.Unfocused += TextBox_Unfocused;
        }
        private void TextBox_Unfocused(object? sender, FocusEventArgs e)
        {
            double acc;
            editingNumericAccuracy = true;
            if (!cmbQualitative.IsFocused)
            {
                 txtQuantitative.Focus();
            }
            if (!double.TryParse(txtQuantitative.Text, out acc))
            {
                cmbQualitative.SelectedItem = null;
                txtQuantitative.Text = "";
                txtQuantitative.BackgroundColor = Colors.White;
                txtQuantitative.TextColor = Colors.Black;
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
                    txtQuantitative.BackgroundColor = Colors.White;
                    txtQuantitative.TextColor = Colors.Black;
                }
            }
            editingNumericAccuracy = false;
        }
        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editingNumericAccuracy)
                return;
            if (cmbQualitative.SelectedItem != null)
            {
                QualitativeAccuracy qa = (QualitativeAccuracy)(cmbQualitative.SelectedItem);
                int acc = (int)qa;
                txtQuantitative.Text = (acc).ToString();
                txtQuantitative.BackgroundColor = AccuracyBackColor(acc);
                txtQuantitative.TextColor = AccuracyForeColor(acc);
                // the value (int) associated with the QualitativeAccuracy is given to the numerical accuracy
                int accuracyNumber = (int)qa;
            }
        }
        internal Color AccuracyBackColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Colors.Red;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + 5)
            {
                c = Colors.DarkRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + 5)
            {
                c = Colors.MediumVioletRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + 5)
            {
                c = Colors.OrangeRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + 5)
            {
                c = Colors.Orange;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + 5)
            {
                c = Colors.Yellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + 5)
            {
                c = Colors.YellowGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + 5)
            {
                c = Colors.GreenYellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                c = Colors.Lime;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding + 5)
            {
                c = Colors.DarkSeaGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + 5)
            {
                c = Colors.Green;
            }
            else
            {
                c = Colors.MintCream;
            }
            return c;
        }
        internal Color AccuracyForeColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Colors.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + 5)
            {
                c = Colors.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + 5)
            {
                c = Colors.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + 5)
            {
                c = Colors.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + 5)
            {
                c = Colors.White;
            }
            else
            {
                c = Colors.White;
            }
            return c;
        }
    }
}

