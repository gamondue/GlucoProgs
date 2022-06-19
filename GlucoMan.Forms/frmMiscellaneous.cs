using SharedGlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmMiscellaneous : Form
    {
        bool canModify = true; 
        public frmMiscellaneous()
        {
            InitializeComponent();
        }
        private void frmMiscellaneous_Load(object sender, EventArgs e)
        {

        }
        private void txt_mgPerdL_TextChanged(object sender, EventArgs e)
        {
            double value;
            double.TryParse(txt_mgPerdL.Text, out value);
            if (canModify)
            {
                canModify = false;
                txt_mmolPerL.Text = Common.mgPerdL_To_mmolPerL(value).ToString("0.00");
            }
            else
            {
                canModify = true;
            }
        }
        private void txt_mmolPerL_TextChanged(object sender, EventArgs e)
        {
            double value;
            double.TryParse(txt_mmolPerL.Text, out value);
            if (canModify)
            {
                canModify = false;
                txt_mgPerdL.Text = Common.mmolPerL_To_mgPerdL(value).ToString("0");
            }
            else
            {
                canModify = true; 
            }
        }
        private void btnResetDatabase_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("ATTENTION: should I delete the WHOLE database? All data will be lost!", 
                "ERASING DATABASE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2)
                == DialogResult.Yes)
            {
                BL_General bl = new SharedGlucoMan.BusinessLayer.BL_General();
                // deleting the file will result automatically in a reset of the database 
                bl.DeleteDatabase(); 
            }
        }
        private void btnCopyDatabase_Click(object sender, EventArgs e)
        {
            string oneDrivePath = Common.PathUser.Substring(0, Common.PathUser.LastIndexOf(@"\")); 
            File.Copy(Common.PathAndFileDatabase, Path.Combine(oneDrivePath, @"OneDrive\GlucoMan\", Common.NameDatabase)); 
        }
    }
}
