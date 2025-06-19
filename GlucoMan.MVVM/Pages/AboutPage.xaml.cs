namespace GlucoMan.MVVM.Pages;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();

        string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        lblAppName.Text += " " + version;
    }
    public async void btnExit_Click(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///main"); 
    }
}