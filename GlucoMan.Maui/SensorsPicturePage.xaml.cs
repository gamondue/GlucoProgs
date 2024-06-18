namespace GlucoMan.Maui;
using GlucoMan.BusinessLayer;

public partial class SensorsPicturePage : ContentPage
{
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
    CirclesDrawable allCircles;
    bool editing = false;
    InsulinInjection currentInjection;
    Point currentNearestReferencePosition;
    List<InsulinInjection> allRecentInjections;
    public SensorsPicturePage(ref InsulinInjection currentInjection)
	{
        InitializeComponent();
        this.currentInjection = currentInjection;
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

        // read the last days of sensors' positions
        allRecentInjections = bl.GetInjections(
           DateTime.Now.Subtract(new TimeSpan(12 * 7, 0, 0, 0, 0, 0)),
           DateTime.Now, Zone: Common.ZoneOfPosition.Sensor);

        // copy the coordinates of the sensors' positions into the Drawable object
        allCircles.LoadCoordinates(allRecentInjections);

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
                // the method will give back the nearest point to the click point
                currentNearestReferencePosition = allCircles.AddPoint((Point)relativeToContainerPosition, editing);
                currentInjection.PositionX = currentNearestReferencePosition.X;
                currentInjection.PositionY = currentNearestReferencePosition.Y;
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
    private void btnSaveInjection_Clicked(object sender, EventArgs e)
    {
        // we put the coordinate of the point in the injection object
        currentInjection.PositionX = currentNearestReferencePosition.X;
        currentInjection.PositionY = currentNearestReferencePosition.Y;
        // we pass the coordinates of the point to the calling Page, through the injection object
        currentInjection.Zone = Common.ZoneOfPosition.Sensor;

        // we save the injection with the coordinates of the points
        // uncomment if we want to really save the injection in the database
        // if we uncomment we will have to avoid to save the same injection twice
        //// if we have no date we put the current date
        //if (currentInjection.Timestamp.DateTime == null
        //    || currentInjection.Timestamp.DateTime == new DateTime(1, 1, 1))
        //    currentInjection.Timestamp.DateTime = DateTime.Now;
        //bl.SaveOneInjection(currentInjection);
    }
    private void btnClear_Click(object sender, EventArgs e)
    {
        allCircles.ClearAll();
        this.cerchiGraphicsView.Invalidate();
    }
    private void btnSavePoints_Clicked(object sender, EventArgs e)
    {
        allCircles.SaveCoordinatesToFile();
    }
}