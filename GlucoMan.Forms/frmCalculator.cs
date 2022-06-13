
namespace GlucoMan.Forms
{
    public partial class frmCalculator : Form
    {
        private string nextOperator;

        public double? Result { get; internal set; }

        public frmCalculator(double? InitialValue)
        {
            InitializeComponent();

            if (InitialValue != null)
                Result = InitialValue;
            else
                Result = 0;
            txtDisplay.Text = InitialValue.ToString();
        }
        private void frmCalculator_Load(object sender, EventArgs e)
        {

        }
        private void btnDigit_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void btnDigit_Click(object sender, EventArgs e)
        {
            try
            {
                txtDisplay.Text += ((Button)sender).Text;
            }
            catch { }
        }
        private void btnMinus_Click(object sender, EventArgs e)
        {
            Operation(txtDisplay.Text);
            nextOperator = "-";
        }
        private void btnPlus_Click(object sender, EventArgs e)
        {
            Operation(txtDisplay.Text);
            nextOperator = "+";
        }
        private void btnDivision_Click(object sender, EventArgs e)
        {
            Operation(txtDisplay.Text);
            nextOperator = "/";
        }
        private void btnMultiplication_Click(object sender, EventArgs e)
        {
            Operation(txtDisplay.Text);
            nextOperator = "*";
        }
        private void btnEqual_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Operation(string text)
        {
            double value;
            if (double.TryParse(text, out value))
            {
                switch (nextOperator)
                {
                    case "-":
                        Result = Result - value;
                        break;
                    case "+":
                        Result = Result + value;
                        break;
                    case "*":
                        Result = Result * value;
                        break;
                    case "/":
                        Result = Result / value;
                        break;
                    default:
                        Result = Result;
                        break; 
                }
            }
            else
            {
                txtDisplay.Text = "";
            }
            //txtDisplay.Text = "";
            txtDisplay.Text = Result.ToString();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "0"; 
        }
    }
}
