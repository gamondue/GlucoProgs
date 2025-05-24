using GlucoMan.BusinessObjects;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class ClickableImagePage : ContentPage
{
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
    CirclesDrawable allCircles;
    bool editing = false;
    Injection currentInjection;
    Point currentNearestReferencePosition;
    List<Injection> allRecentInjections;
    List<PositionOfInjection> allReferencePositions;
    private bool firstPass = true;

    public ClickableImagePage(ref Injection currentInjection)
	{
		InitializeComponent();
        this.currentInjection = currentInjection;
#if !DEBUG
                // in release mode we hide the stack layout that allows to 
                // edit the coordinates of the reference points
                // TODO don't hide but "unregister" the stack layout
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
                    break;
                }
            case Common.ZoneOfPosition.Back:
                {
                    // Load the image "back.png" from resources
                    imgToBeTapped.Source = "back.png";
                    this.Title = "Back past injections";
                    break;
                }
            case Common.ZoneOfPosition.Hands:
                {
                    // Load the image "hands.png" from resources
                    imgToBeTapped.Source = "hands.png";
                    this.Title = "Hand blood samples positions";
                    break;
                }
            case Common.ZoneOfPosition.Sensor:
                {
                    // Load the image "sensor.png" from resources
                    imgToBeTapped.Source = "arms_back.png";
                    this.Title = "Sensors' past positions";
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
            // Inizializza CirclesDrawable solo quando Width e Height sono definiti
            allCircles = new CirclesDrawable(imgToBeTapped.Width, imgToBeTapped.Height);
            allCircles.Type = CirclesDrawable.PointType.Front;
            //if (firstPass)
            //{
                // bind the Drawable to the GraphicsView
                cerchiGraphicsView.Drawable = allCircles;
                cerchiGraphicsView.Background = Color.FromRgba(255, 255, 0, 100);
                var children = cerchiGraphicsView.GetChildElements(new Point(100, 100));
                firstPass = false;
            //}
            // Carica i dati necessari
            LoadTheReferencePositions();
            LoadTheLastSensorsPositions();

            //// Rimuovi l'handler per evitare chiamate multiple
            //imgToBeTapped.SizeChanged -= ImgToBeTapped_SizeChanged;

            // redraw the circles
            cerchiGraphicsView.Invalidate();
        }
    }
    private void LoadTheLastSensorsPositions()
    {
        SetButtonsVisibility();

        // read the last days of sensors' positions
        allRecentInjections = bl.GetInjections(
           DateTime.Now.Subtract(new TimeSpan(12 * 7, 0, 0, 0, 0, 0)),
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
        //// we pass the coordinates of the point to the calling Page, through the injection object
        //currentInjection.Zone = Common.ZoneOfPosition.Front;
        // if we have no date we put the current time
        if (currentInjection.Timestamp.DateTime == null
            || currentInjection.Timestamp.DateTime == new DateTime(1, 1, 1))
        {
            currentInjection.Timestamp.DateTime = DateTime.Now;
        }
        // we save the injection with the coordinates of the points
        bl.SaveOneInjection(currentInjection);
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