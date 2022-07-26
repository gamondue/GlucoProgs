using SharedGlucoMan.BusinessLayer;

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
                BL_General blGeneral = new SharedGlucoMan.BusinessLayer.BL_General();
                // deleting the file will result automatically in a reset of the database 
                blGeneral.DeleteDatabase(); 
            }
        }
        private void btnCopyDatabase_Click(object sender, EventArgs e)
        {
            string oneDrivePath = Common.PathUser.Substring(0, Common.PathUser.LastIndexOf(@"\")); 
            File.Copy(Common.PathAndFileDatabase, Path.Combine(oneDrivePath, @"OneDrive\GlucoMan\", Common.NomeFileDatabase)); 
        }
        private void btnShowErrorLog_Click(object sender, EventArgs e)
        {
            try
            {
                string fileContent = File.ReadAllText(Common.LogOfProgram.ErrorsFile);
                frmShowText f = new frmShowText(fileContent);
                f.Show();
            } catch
            {
                MessageBox.Show("File not existing or not accessible."); 
            }
        }
        private void btnDeleteErrorLog_Click(object sender, EventArgs e)
        {
            File.Delete(Common.LogOfProgram.ErrorsFile);
            MessageBox.Show("Done!"); 
        }
    }
}
