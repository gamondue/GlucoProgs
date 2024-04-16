namespace GlucoMan.Maui;

public partial class HandsPicturePage : ContentPage
{
	public HandsPicturePage()
	{
		InitializeComponent();
	}
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        // !!!! NON PARTE. CAPIRE PERCHE' !!!! 
        Point? relativeToContainerPosition = e.GetPosition((View)sender);
        if (relativeToContainerPosition.HasValue)
        {
            double x = relativeToContainerPosition.Value.X;
            double y = relativeToContainerPosition.Value.Y;
        }
    }
}