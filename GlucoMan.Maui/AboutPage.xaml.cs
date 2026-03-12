namespace GlucoMan.Maui;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();

        string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        lblAppName.Text += " " + version;
    }
    public void btnExit_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}