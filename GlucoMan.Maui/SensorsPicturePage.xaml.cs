namespace GlucoMan.Maui;

public partial class SensorsPicturePage : ContentPage
{
    CirclesDrawable allCircles;
    bool editing = false;
    public SensorsPicturePage()
	{
        InitializeComponent();

#if !DEBUG
        // in release mode we hide the stack layout that allows to 
        // edit the coordinates of the reference points
        // TODO don't hide but "unregister" the stack layout
        stackEditingOptions.IsVisible = false;
#endif
        // we define a new object that represents the set of circles to be drawn
        // into the GraphcsView. It derives from the Interface IDrawable
        allCircles = new CirclesDrawable();
        allCircles.Type = CirclesDrawable.PointType.Sensor;

        // collegamento del Drawable al GraphicsView
        cerchiGraphicsView.Drawable = allCircles;
        cerchiGraphicsView.Background = Color.FromRgba(255, 255, 0, 100);
        var children = cerchiGraphicsView.GetChildElements(new Point(100, 100));

        // reset of the GraphcsView that will be redrawn by method Draw in allCircles
        cerchiGraphicsView.Invalidate();
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        allCircles.IsCallerEditing = editing;
        // Position relative to the container view (the image).
        // The origin point is at the top-left corner of the image.
        Point? relativeToContainerPosition = e.GetPosition((View)sender);
        if (relativeToContainerPosition.HasValue)
        {
            if (DeleteCheckBox.IsChecked)
            {
                // if delete checkbox is enabled we have to delete the circle that is nearest
                // to the click point 

                allCircles.RemovePointIfNear(relativeToContainerPosition.Value);
            }
            else
            {
                // passes the position of the click to the creator method of a new circle 
                // the constructor will calculate the position of the center of the circle
                allCircles.AddPoint((Point)relativeToContainerPosition, editing);
            }

            // clears the GraphicsView that will be redrawn by the Draw method
            // the system will launch the Draw() method to redraw
            this.cerchiGraphicsView.Invalidate();
        }
    }
    private void CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        editing = e.Value;
        allCircles.IsCallerEditing = e.Value;
        this.cerchiGraphicsView.Invalidate();
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
        allCircles.SaveCoordinatesToFile();
    }
    private void btnClear_Click(object sender, EventArgs e)
    {
        allCircles.ClearFile();
        this.cerchiGraphicsView.Invalidate();
    }
}