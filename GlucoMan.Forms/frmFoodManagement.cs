using GlucoMan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmFoodManagement : Form
    {
        Food currentFood = new Food();
        List<Food> foods; 

        public frmFoodManagement()
        {
            InitializeComponent();
        }
        private void frmFoodManagement_Load(object sender, EventArgs e)
        {
            // !!!! ????  is it right to get persistence throught the object ???? foods = currentFood.ReadAllFoods();
            //foods = currentFood.OrderBy(n => n.IdFood).ToList();
            gridFoods.DataSource = null;
            gridFoods.DataSource = foods;
        }

        private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
