using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace gamon
{
    internal partial class Accuracy
    {
        TextBox txtQuantitative;
        ComboBox cmbQualitative; 
        bool clickedFlag;
        internal Accuracy (TextBox TextBox, ComboBox Combo)
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
                    GetQualitativeAccuracyGivenQuantitavive(acc);
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
                int accuracyNumber = (int)qa;
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
