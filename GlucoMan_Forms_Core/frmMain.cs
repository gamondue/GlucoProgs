using GlucoMan;
using GlucoMan.BusinessLayer; 
using System;
using System.Windows.Forms;

namespace GlucoMan_Forms_Core
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnHypoPrediction_Click(object sender, EventArgs e)
        {
            frmPredictHypo f = new frmPredictHypo();
            f.Show(); 
        }

        private void btnWeighFood_Click(object sender, EventArgs e)
        {
            frmCorrectionBolus f = new frmCorrectionBolus(); 
            f.Show(); 
        }

        private void btnInsulineCalc(object sender, EventArgs e)
        {
            frmInsulineCalc f = new frmInsulineCalc();
            f.Show(); 
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text += " " + version;
            txtHeaderText.Text = "GlucoMan will be a Glucose Manager program for diabetic persons." +
                "\r\nCurrently it makes just a few calculations, but very useful to the diabetic person that has to make carbohydrate count." +
                "\r\nI will expand the program in the future, including full management of insuline, carboihydrates, and possibly foods." +
                "\r\nDistribution at: https://ingmonti.it/GlucoMan/" +
                "\r\nThe first functions I implemented are those enabled in the following buttons:";
            txtFooterText.Text = "GlucoMan is Free Software by:" +
                "\r\nIng.Gabriele Monti (gamon) - Forlì - Italia"+
                "\r\nLicence: GPL v.2";
        }

        private void btnFoodToHitTargetCarbs_Click(object sender, EventArgs e)
        {
            frmFoodToHitTargetCarbs f = new frmFoodToHitTargetCarbs();
            f.Show(); 
        }

        private void btnChoCount_Click(object sender, EventArgs e)
        {
            frmMeal fc = new frmMeal();
            fc.Show(); 
        }

        private void btnAlarms_Click(object sender, EventArgs e)
        {
            frmAlarms fa = new frmAlarms();
            fa.Show(); 
        }

        private void btnGlucoseMeasurement_Click(object sender, EventArgs e)
        {
            frmGlucose fg = new frmGlucose();
            fg.Show(); 
        }
    }
}
