
namespace GlucoMan.Forms
{
    public partial class frmCalculator : Form
    {
        private string nextOperator;
        private double firstOperand;

        public double Result { get; internal set; }
        public bool ClosedWithOk { get; private set; }

        public frmCalculator(double InitialValue)
        {
            InitializeComponent();

            ClosedWithOk = false; 
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
            nextOperator = "-";
            getOperand();
        }
        private void btnPlus_Click(object sender, EventArgs e)
        {
            nextOperator = "+";
            getOperand();
        }
        private void btnDivision_Click(object sender, EventArgs e)
        {
            nextOperator = "/";
            getOperand(); 
        }
        private void btnMultiplication_Click(object sender, EventArgs e)
        {
            nextOperator = "*";
            getOperand();
        }
        private void btnEqual_Click(object sender, EventArgs e)
        {
            Operation(txtDisplay.Text); 
        }
        private void getOperand()
        {
            if (txtDisplay.Text != "")
            {
                double.TryParse(txtDisplay.Text, out firstOperand);
                txtDisplay.Text = ""; 
            }
        }
        private void Operation(string text)
        {
            double value;
            if (double.TryParse(text, out value))
            {
                switch (nextOperator)
                {
                    case "-":
                        Result = firstOperand - value;
                        break;
                    case "+":
                        Result = firstOperand + value;
                        break;
                    case "*":
                        Result = firstOperand * value;
                        break;
                    case "/":
                        Result = firstOperand / value;
                        break;
                    default:
                        Result = firstOperand;
                        break; 
                }
            }
            else
            {
                txtDisplay.Text = "";
            }
            txtDisplay.Text = Result.ToString("#.########");
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            firstOperand = 0; 
            Result = 0; 
            txtDisplay.Text = "0"; 
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            ClosedWithOk = true; 
            this.Close();
        }
    }
}
