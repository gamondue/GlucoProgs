
namespace GlucoMan.Maui;

public partial class RecipesPage : ContentPage
{
    public RecipesPage()
    {
        InitializeComponent();
    }
    private async void btnRecipeDetails_ClickedAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecipePage());
    }
}