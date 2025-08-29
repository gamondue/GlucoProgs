using GlucoMan.BusinessObjects;
using GlucoMan.BusinessLayer;
using gamon;

namespace GlucoMan.Maui;

public partial class ClickableImagePage : ContentPage
{
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
    CirclesDrawable allCircles;
    bool editing = false;
    Injection currentInjection;
    Microsoft.Maui.Graphics.Point currentNearestReferencePosition;
    List<Injection> allRecentInjections;
    List<PositionOfInjection> allReferencePositions;
    private bool firstPass = true;
    private double circlesVisibilityMaxTimeInDays;

    public ClickableImagePage(ref Injection currentInjection)
	{
		InitializeComponent();
        this.currentInjection = currentInjection;
#if !DEBUG
        // in release mode we hide the stack layout that allows to 
        // edit the coordinates of the reference points
        EditorCheckBox.IsVisible = false;
        LabelEditorCheckBox.IsVisible = false;
#endif
        // add an handler for SizeChanged event
        imgToBeTapped.SizeChanged += ImgToBeTapped_SizeChanged;
        // load the right image
        switch (currentInjection.Zone)
        {
            case Common.ZoneOfPosition.Front:
                {
                    // Load the image "front.png" from resources
                    imgToBeTapped.Source = "front.png";
                    this.Title = "Front past injections";
                    circlesVisibilityMaxTimeInDays = 60.0 / 3 + 1; // 60 positions / the 3 fast injections per day
                    break;
                }
            case Common.ZoneOfPosition.Back:
                {
                    // Load the image "back.png" from resources
                    imgToBeTapped.Source = "back.png";
                    this.Title = "Back past injections";
                    circlesVisibilityMaxTimeInDays = 40.0 / 1 + 1; // 40 positions / the 1 slow injections per day
                    break;
                }
            case Common.ZoneOfPosition.Hands:
                {
                    // Load the image "hands.png" from resources
                    imgToBeTapped.Source = "hands.png";
                    this.Title = "Hand blood samples positions";
                    circlesVisibilityMaxTimeInDays = 100 / 4.5 + 1; // 100 positions, 4.5 per day 
                    break;
                }
            case Common.ZoneOfPosition.Sensor:
                {
                    // Load the image "sensor.png" from resources
                    imgToBeTapped.Source = "arms_back.png";
                    this.Title = "Sensors' past positions";
                    circlesVisibilityMaxTimeInDays = 2 * 7 * 6 + 7; // 2 weeks by 6 positions (+1 week)
                    break;
                }
            //default:
            //    {
            //        // Load a default image if the zone is not specified
            //        imgToBeTapped.Source = ImageSource.FromFile("default.png");
            //        this.Title = "Default past injections";
            //        break;
            //    }
        }
    }
    private void ImgToBeTapped_SizeChanged(object sender, EventArgs e)
    {
        if (imgToBeTapped.Width > 0 && imgToBeTapped.Height > 0)
        {
            // Initialize CirclesDrawable only when Width and Height are defined
            allCircles = new CirclesDrawable(imgToBeTapped.Width, imgToBeTapped.Height, 
                currentInjection.IdInjection, circlesVisibilityMaxTimeInDays);
            // ???? why the following works ????
            allCircles.Type = CirclesDrawable.PointType.Front;
            
            // bind the Drawable to the GraphicsView
            cerchiGraphicsView.Drawable = allCircles;
            cerchiGraphicsView.Background = Color.FromRgba(255, 255, 0, 100);
            var children = cerchiGraphicsView.GetChildElements(new Microsoft.Maui.Graphics.Point(100, 100));
            firstPass = false;
            
            // Load necessary data
            LoadTheReferencePositions();
            LoadTheLastSensorsPositions();

            // redraw the circles
            cerchiGraphicsView.Invalidate();
        }
    }
    private void LoadTheLastSensorsPositions()
    {
        SetButtonsVisibility();
        // read the last days of sensors' positions
        allRecentInjections = bl.GetInjections(
           DateTime.Now.Subtract(new TimeSpan((int)circlesVisibilityMaxTimeInDays + 1, 0, 0, 0, 0, 0)),
           DateTime.Now, Zone: currentInjection.Zone);

        // copy the coordinates of the sensors' positions into the Drawable object
        allCircles.LoadInjectionsCoordinates(allRecentInjections);
        // reset of the GraphcsView that will be redrawn by method Draw in allCircles
        cerchiGraphicsView.Invalidate();
    }
    private void LoadTheReferencePositions()
    {
        // read all the reference positions
        allReferencePositions = bl.GetReferencePositions(currentInjection.Zone);
        allCircles.LoadReferenceCoordinates(allReferencePositions);
    }
    private void SetButtonsVisibility()
    {
        if (editing)
        {
            btnSavePosition.IsVisible = false;
            btnForgetPosition.IsVisible = false;
            DeleteCheckBox.IsVisible = true;
            lblDelete.IsVisible = true;
            btnSavePoints.IsVisible = true;
            btnClear.IsVisible = true;
        }
        else
        {
            btnSavePosition.IsVisible = true;
            btnForgetPosition.IsVisible = true;
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
        Microsoft.Maui.Graphics.Point? relativeToContainerPosition = e.GetPosition((View)sender);
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
                currentNearestReferencePosition = allCircles.AddPoint(
                    (Microsoft.Maui.Graphics.Point)relativeToContainerPosition, editing);
                currentInjection.PositionX = currentNearestReferencePosition.X;
                currentInjection.PositionY = currentNearestReferencePosition.Y;
            }
            // clears the GraphicsView that then will redraw with the Draw method
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
    private void BtnSavePosition_Clicked(object sender, EventArgs e)
    {
        // we pass the coordinates of the point to the calling Page, through the injection object
        // we put the coordinate of the point in the injection object
        currentInjection.PositionX = allCircles.NormalizeXPosition(currentNearestReferencePosition.X);
        currentInjection.PositionY = allCircles.NormalizeYPosition(currentNearestReferencePosition.Y);
        // the zone and other data are already included in the currentInjection that has been passed
        // close the page
        this.Navigation.PopAsync();
    }
    private void BtnForgetPosition_Clicked(object sender, EventArgs e)
    {
        // if the user exits with another path the current position is forgotten
        this.Navigation.PopAsync();
    }
    private void BtnClearReferencePoints_Click(object sender, EventArgs e)
    {
        allCircles.ClearAll(currentInjection.Zone);
        this.cerchiGraphicsView.Invalidate();
    }
    private void BtnSaveReferencePoints_Clicked(object sender, EventArgs e)
    {
        allCircles.SaveReferenceCoordinates(currentInjection.Zone
                        , imgToBeTapped.Width, imgToBeTapped.Height);
    }
}