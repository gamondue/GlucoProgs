using System;
using System.Windows.Forms;
using GlucoMan;
using GlucoMan.BusinessLayer;

namespace GlucoMan_Forms_Core
{
    public partial class frmPredictHypo : Form
    {
        HypoPrediction hypo; 
        public frmPredictHypo()
        {
            InitializeComponent();

            hypo = new HypoPrediction();

            hypo.RestoreData();
            FromClassToUi(hypo);
        }

        private void FromClassToUi(HypoPrediction hypo)
        {
            txtGlucoseTarget.Text = hypo.HypoGlucoseTarget.Text;
            txtGlucoseLast.Text = hypo.GlucoseLast.Text;
            txtGlucosePrevious.Text = hypo.GlucosePrevious.Text;
            txtHourLast.Text = hypo.HourLast.Text;
            txtHourPrevious.Text = hypo.HourPrevious.Text;
            txtMinuteLast.Text = hypo.MinuteLast.Text;
            txtMinutePrevious.Text = hypo.MinutePrevious.Text;
            txtGlucoseSlope.Text = hypo.GlucoseSlope.ToString(); 
        }

        private void FromUiToClass(HypoPrediction hypo)
        {
            hypo.HypoGlucoseTarget.Text = txtGlucoseTarget.Text;
            hypo.GlucoseLast.Text = txtGlucoseLast.Text;
            hypo.GlucosePrevious.Text = txtGlucosePrevious.Text;
            hypo.HourLast.Text = txtHourLast.Text;
            hypo.HourPrevious.Text = txtHourPrevious.Text;
            hypo.MinuteLast.Text = txtMinuteLast.Text;
            hypo.MinutePrevious.Text = txtMinutePrevious.Text;
        }

        private void btnNow_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            txtHourLast.Text = now.Hour.ToString();
            txtMinuteLast.Text = now.Minute.ToString();
            txtGlucosePrevious.Focus(); 
        }

        private void txtGlucoseLast_Leave(object sender, EventArgs e)
        {
            btnNow_Click(null, null); 
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            FromUiToClass(hypo); 
            DateTime? finalTime = hypo.PredictHypoTime();
            if (finalTime != null)
            {
                txtPredictedHour.Text = ((DateTime)finalTime).Hour.ToString();
                txtPredictedMinute.Text = ((DateTime)finalTime).Minute.ToString();
            }
            else
            {
                Console.Beep();
                txtPredictedHour.Text = "XXXX";
                txtPredictedMinute.Text = "XXXX";
            }
            txtGlucoseSlope.Text = hypo.GlucoseSlope.Text; 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //btnNow_Click(null, null);

            txtGlucoseLast.Focus(); 
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            txtGlucosePrevious.Text = txtGlucoseLast.Text;
            txtHourPrevious.Text = txtHourLast.Text;
            txtMinutePrevious.Text = txtMinuteLast.Text;

            txtGlucoseLast.Text = ""; 
            btnNow_Click(null,null);
            txtGlucoseLast.Focus();
        }

        private void frmPredictHypo_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass(hypo); 
            hypo.SaveData(); 
        }
    }
}
