using System.Xml.Linq;

namespace GlucoMan.Maui;

public partial class ShowTextPage : ContentPage
{
    private string fileContent;

    public ShowTextPage(string fileContent)
    {
        InitializeComponent();
        txtText.Text = fileContent;
    }
}