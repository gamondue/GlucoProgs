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
            frmWeighFood f = new frmWeighFood(); 
            f.Show(); 
        }

        private void btnInsulineCalc(object sender, EventArgs e)
        {
            frmInsulineCalc f = new frmInsulineCalc();
            f.Show(); 
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnFoodToHitTargetCarbs_Click(object sender, EventArgs e)
        {
            frmFoodToHitTargetCarbs f = new frmFoodToHitTargetCarbs();
            f.Show(); 
        }
    }
}
