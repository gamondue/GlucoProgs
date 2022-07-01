using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text += " " + Common.Version;
            txtHeaderText.Text = "GlucoMan will be a Glucose Manager program for diabetic persons." +
                "\r\nCurrently it makes just a few calculations, but very useful to the diabetic person that has to make carbohydrate count." +
                "\r\nI will expand the program in the future, including full management of insulin, carboihydrates, and possibly foods." +
                "\r\nDistribution at: https://ingmonti.it/GlucoMan/" +
                "\r\nThe first functionalities I implemented are those enabled in the following buttons:";
            txtFooterText.Text = "GlucoMan is Free Software by:" +
                "\r\nIng.Gabriele Monti (gamon) - Forlì - Italia" +
                "\r\nLicence: GPL v.2";
        }
        private void btnGlucoseMeasurement_Click(object sender, EventArgs e)
        {
            frmGlucose fg = new frmGlucose();
            fg.Show();
        }
        private void btnMeals_Click(object sender, EventArgs e)
        {
            frmMeals mm = new frmMeals();
            mm.Show();
        }
        private void btnWeighFood_Click(object sender, EventArgs e)
        {

        }
        private void btnInsulinCalc(object sender, EventArgs e)
        {
            frmInsulinCalc f = new frmInsulinCalc();
            f.Show();
        }
        private void btnFoodToHitTargetCarbs_Click(object sender, EventArgs e)
        {
            frmFoodToHitTargetCarbs f = new frmFoodToHitTargetCarbs();
            f.Show();
        }
        private void btnHypoPrediction_Click(object sender, EventArgs e)
        {
            frmHypoTimePrediction f = new frmHypoTimePrediction();
            f.Show(); 
        }
        private void btnAlarms_Click(object sender, EventArgs e)
        {
            frmAlarms fa = new frmAlarms();
            fa.Show(); 
        }
        private void bntMiscellaneous_Click(object sender, EventArgs e)
        {
            frmMiscellaneous f = new frmMiscellaneous();
            f.Show(); 
        }
        private void btnInjectionSites_Click(object sender, EventArgs e)
        {
            frmInjectionSite f = new frmInjectionSite();
            f.Show();
        }
        private void btnFoods_Click(object sender, EventArgs e)
        {
            frmFoods f = new frmFoods();
            f.Show(); 
        }
    }
}
