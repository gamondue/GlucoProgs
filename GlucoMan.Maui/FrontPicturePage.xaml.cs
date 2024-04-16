namespace GlucoMan.Maui;

public partial class FrontPicturePage : ContentPage
{
	public FrontPicturePage()
	{
		InitializeComponent();
	}
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        // Position relative to the container view (the image).
        // The origin point is at the top-left corner of the image.
        Point? relativeToContainerPosition = e.GetPosition((View)sender);
        if (relativeToContainerPosition.HasValue)
        {
            double x = relativeToContainerPosition.Value.X;
            double y = relativeToContainerPosition.Value.Y;
        }
    }
}