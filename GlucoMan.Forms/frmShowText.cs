namespace GlucoMan.Forms
{
    public partial class frmShowText : Form
    {
        string textToShow; 
        public frmShowText(string TextToShow)
        {
            InitializeComponent();

            textToShow = TextToShow; 
            this.Text = "";
        }
        private void frmShowText_Load(object sender, EventArgs e)
        {
            txtText.Text = textToShow;
        }
    }
}
