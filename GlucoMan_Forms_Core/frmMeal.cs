using System;
using System.Windows.Forms;
using GlucoMan; 

namespace GlucoMan_Forms_Core
{
    public partial class frmMeal : Form
    {
        public frmMeal()
        {
            InitializeComponent();
        }
        private void viewFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void viewFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Food f = new Food(); 
            frmFood fd = new frmFood(f);
            fd.Show(); 
        }

        private void frmMeal_Load(object sender, EventArgs e)
        {

        }
    }
}
