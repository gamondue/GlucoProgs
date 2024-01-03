using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmInjections : Form
    {
        private InsulinInjection CurrentInjection = new InsulinInjection();
        BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
        List<InsulinInjection> allInjections; 
        public frmInjections()
        {
            InitializeComponent();
        }
        private void frmInjections_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        private void FromClassToUi()
        {
            if (CurrentInjection.IdInsulinInjection != null)
                txtIdInsulinInjection.Text = CurrentInjection.IdInsulinInjection.ToString();
            else
                txtIdInsulinInjection.Text = ""; 
            txtInsulinActual.Text = CurrentInjection.InsulinValue.Text;
            txtInsulinCalculated.Text = CurrentInjection.InsulinCalculated.Text;
            dtpInjectionDate.Value = ((DateTime) CurrentInjection.Timestamp.DateTime); 
            dtpInjectionTime.Value = ((DateTime) CurrentInjection.Timestamp.DateTime);
            txtNotes.Text = CurrentInjection.Notes;
            if (CurrentInjection.IdTypeOfInsulinSpeed == (int)Common.TypeOfInsulinSpeed.RapidActing)
                rdbFastInsulin.Checked = true;
            else if (CurrentInjection.IdTypeOfInsulinSpeed == (int)Common.TypeOfInsulinSpeed.ShortActing)
                rdbSlowInsulin.Checked = true;
            else
            {
                rdbFastInsulin.Checked = false;
                rdbSlowInsulin.Checked = false;
            }
        }
        private void FromUiToClass()
        {
            CurrentInjection.IdInsulinInjection = SqlSafe.Int(txtIdInsulinInjection.Text); 
            CurrentInjection.InsulinValue.Text = txtInsulinActual.Text;
            CurrentInjection.InsulinCalculated.Text = txtInsulinCalculated.Text;
            DateTime instant = new DateTime(
                dtpInjectionDate.Value.Date.Year, dtpInjectionDate.Value.Date.Month, dtpInjectionDate.Value.Date.Day,
                dtpInjectionTime.Value.Hour, dtpInjectionTime.Value.Minute, dtpInjectionTime.Value.Second);
            CurrentInjection.Timestamp.DateTime = instant;
            CurrentInjection.Notes = txtNotes.Text;
            if (rdbFastInsulin.Checked)
                CurrentInjection.IdTypeOfInsulinSpeed = (int)Common.TypeOfInsulinSpeed.RapidActing;
            else if (rdbSlowInsulin.Checked)
                CurrentInjection.IdTypeOfInsulinSpeed = (int)Common.TypeOfInsulinSpeed.ShortActing;
            else
                CurrentInjection.IdTypeOfInsulinSpeed = (int)Common.TypeOfInsulinSpeed.NotSet;
        }
        private void RefreshGrid()
        {
            DateTime now = DateTime.Now; 
            allInjections = bl.GetInjections(now.AddMonths(-2), now, Common.TypeOfInsulinSpeed.NotSet);
            gridInjections.DataSource = allInjections;
            gridInjections.Refresh(); 
        }
        private void btnNow_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now; 
            dtpInjectionDate.Value = now;
            dtpInjectionTime.Value = now;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CurrentInjection.IdInsulinInjection == null)
            {
                MessageBox.Show("Choose the injection to modify");
                return;
            }
            if (chkNowInAdd.Checked)
                dtpInjectionTime.Value = DateTime.Now;
            FromUiToClass();
            bl.SaveOneInjection(CurrentInjection);
            RefreshGrid();
        }
        private void gridInjections_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gridInjections_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // select the row
                gridInjections.Rows[e.RowIndex].Selected = true;
                // make the clicked row current
                CurrentInjection = (InsulinInjection)allInjections[e.RowIndex];
                FromClassToUi();
            }
        }
        private void btnAddInjection_Click(object sender, EventArgs e)
        {
            if (chkNowInAdd.Checked)
                dtpInjectionTime.Value = DateTime.Now;
            FromUiToClass();
            // erase Id to save a new record
            CurrentInjection.IdInsulinInjection = null;
            bl.SaveOneInjection(CurrentInjection);
            RefreshGrid();
        }
        private void btnRemoveInjection_Click(object sender, EventArgs e)
        {
            if (gridInjections.SelectedRows.Count > 0)
            {
                int rowIndex = gridInjections.SelectedRows[0].Index;
                InsulinInjection inj = (InsulinInjection)allInjections[rowIndex];
                if (MessageBox.Show(string.Format("Should I delete the injection of {1}, insulin {0}, Id {2}?",
                    inj.InsulinValue,
                    inj.Timestamp,
                    inj.IdInsulinInjection), "",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bl.DeleteOneInjection(inj);
                    RefreshGrid();
                }
            }
            else
            {
                MessageBox.Show("Choose a measurement to delete");
                return;
            }
            RefreshGrid();
        }
    }
}
