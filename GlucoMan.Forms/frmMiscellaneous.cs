using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmMiscellaneous : Form
    {
        BL_General blGeneral = new BL_General();
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
            if (MessageBox.Show("ATTENTION: should I delete the WHOLE database? All data will be lost!",
                "ERASING DATABASE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.Yes)
            {
                // deleting the database file
                // after deletion the software will automatically re-create the database
                if (!blGeneral.DeleteDatabase())
                {
                    MessageBox.Show("Error in deleting file. File NOT deleted",
                      "ERASING DATABASE", MessageBoxButtons.OK);
                }
            }
        }
        private void btnCopyDatabase_Click(object sender, EventArgs e)
        {
            if (!blGeneral.ExportProgramsFiles())
            {
                MessageBox.Show("Error in exporting program's files. NOT all files copied, check logs",
                  "Exporting program's files", MessageBoxButtons.OK);
            }
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
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Please put a database named 'import.sqlite' in the same " +
                "folder where this program exports its data.\nShould we continue in the process?", 
                "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (!blGeneral.ImportDatabaseFromExternal(Common.PathAndFileDatabase,
                    Path.Combine(Common.PathImportExport, "import.sqlite")))
                {
                    MessageBox.Show("Error in importing form import-sqlite to app's database", "");
                }
            }
        }
        private void btnReadDatabase_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Please put a database named 'readGlucoman.sqlite' in the same " +
                "folder where this program exports its data." +
                "\nAttention, this file will replace the database! " +
                "\nShould we continue in the process?",
                "Read database from external folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (!blGeneral.ReadDatabaseFromExternal(Common.PathAndFileDatabase,
                    Path.Combine(Common.PathImportExport, "readGlucoman.sqlite")))
                {
                    MessageBox.Show("Error in reading database from external", "Error!");
                }
            }
        }
    }
}
