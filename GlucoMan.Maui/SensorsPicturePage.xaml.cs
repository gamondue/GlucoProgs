namespace GlucoMan.Maui;
using GlucoMan.BusinessLayer;
using GlucoMan.BusinessObjects;
using System.Xml.Linq;

public partial class SensorsPicturePage : ContentPage
{
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
    CirclesDrawable allCircles;
    bool editing = false;
    Injection currentInjection;
    Point currentNearestReferencePosition;
    List<Injection> allRecentInjections;
    List<PositionOfInjection> allReferencePositions;
    public SensorsPicturePage(ref Injection currentInjection)
	{
        InitializeComponent();
        this.currentInjection = currentInjection;
#if !DEBUG
                 in release mode we hide the stack layout that allows to 
                 edit the coordinates of the reference points
                 TODO don't hide but "unregister" the stack layout
                EditorCheckBox.IsVisible = false;
                LabelEditorCheckBox.IsVisible = false;
#endif
        // Fix: Use the Width and Height properties of the Image control
        allCircles = new CirclesDrawable(imgToBeTapped.Width, imgToBeTapped.Height);
        allCircles.Type = CirclesDrawable.PointType.Sensor;

        LoadTheReferencePositions();
        LoadTheLastSensorsPositions();
    }
    private void LoadTheLastSensorsPositions()
    {
        SetButtonsVisibility();
        // bind the Drawable to the GraphicsView
        cerchiGraphicsView.Drawable = allCircles;
        cerchiGraphicsView.Background = Color.FromRgba(255, 255, 0, 100);
        var children = cerchiGraphicsView.GetChildElements(new Point(100, 100));

        // read the last days of sensors' positions
        allRecentInjections = bl.GetInjections(
           DateTime.Now.Subtract(new TimeSpan(12 * 7, 0, 0, 0, 0, 0)),
           DateTime.Now, Zone: Common.ZoneOfPosition.Sensor);

        // copy the coordinates of the sensors' positions into the Drawable object
        allCircles.LoadInjectionsCoordinates(allRecentInjections);

        // reset of the GraphcsView that will be redrawn by method Draw in allCircles
        cerchiGraphicsView.Invalidate();
    }
    private void LoadTheReferencePositions()
    {
        // read all the reference positions
        allReferencePositions = bl.GetReferencePositions(Common.ZoneOfPosition.Sensor);
        allCircles.LoadReferenceCoordinates(allReferencePositions);
    }
    private void SetButtonsVisibility()
    {
        if (editing)
        {
            btnSaveInjection.IsVisible = false;
            DeleteCheckBox.IsVisible = true;
            lblDelete.IsVisible = true;
            btnSavePoints.IsVisible = true;
            btnClear.IsVisible = true;
        }
        else
        {
            btnSaveInjection.IsVisible = true;
            DeleteCheckBox.IsVisible = false;
            lblDelete.IsVisible = false;
            btnSavePoints.IsVisible = false;
            btnClear.IsVisible = false;
        }
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
        SetButtonsVisibility();
        allCircles.IsCallerEditing = e.Value;
        this.cerchiGraphicsView.Invalidate();
    }
    private void BtnSaveInjection_Clicked(object sender, EventArgs e)
    {
        // we put the coordinate of the point in the injection object
        currentInjection.PositionX = currentNearestReferencePosition.X;
        currentInjection.PositionY = currentNearestReferencePosition.Y;
        // we pass the coordinates of the point to the calling Page, through the injection object
        currentInjection.Zone = Common.ZoneOfPosition.Sensor;
        // if we have no date we put the current time
        if (currentInjection.Timestamp.DateTime == null
            || currentInjection.Timestamp.DateTime == new DateTime(1, 1, 1))
        {
            currentInjection.Timestamp.DateTime = DateTime.Now;
        }
        // we save the injection with the coordinates of the points
        bl.SaveOneInjectionNormalizingXandY(currentInjection
            , imgToBeTapped.Width, imgToBeTapped.Height);
    }
    private void BtnClearReferencePoints_Click(object sender, EventArgs e)
    {
        allCircles.ClearAll(Common.ZoneOfPosition.Sensor);
        this.cerchiGraphicsView.Invalidate();
    }
    private void BtnSaveReferencePoints_Clicked(object sender, EventArgs e)
    {
        allCircles.SaveReferenceCoordinates(Common.ZoneOfPosition.Sensor
            , imgToBeTapped.Width, imgToBeTapped.Height);
    }
}