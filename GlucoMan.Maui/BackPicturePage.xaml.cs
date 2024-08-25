//using System.Drawing;
//using Color = Microsoft.Maui.Graphics.Color;

namespace GlucoMan.Maui;
using GlucoMan.BusinessLayer;
public partial class BackPicturePage : ContentPage
{
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
    CirclesDrawable allCircles;
    bool editing = false;
    InsulinInjection currentInjection;
    Point currentNearestReferencePosition;
    List<InsulinInjection> allRecentInjections;
    public BackPicturePage(ref InsulinInjection currentInjection)
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
        allCircles.Type = CirclesDrawable.PointType.Back;

        // connection of the Drawable to GraphicsView
        cerchiGraphicsView.Drawable = allCircles;
        cerchiGraphicsView.Background = Color.FromRgba(255, 255, 0, 100);
        var children = cerchiGraphicsView.GetChildElements(new Point(100, 100));

        // read the last days of injections in the back zone
        allRecentInjections = bl.GetInjections(
           DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0, 0, 0)),
           DateTime.Now, Zone: Common.ZoneOfPosition.Back);

        // copy the coordinates of the injections into the Drawable object
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
            //    float spanHue;          // differenza di tinta da coprire
            //  float spanSaturation;   // differenza di saturazione da coprire
            //  float spanLuminance;    // differenza di saturazione da coprire
            //   spanHue = finalColor.GetHue() - initialColor.GetHue(); // differenza di tinta da coprire
            //  //spanSaturation = coloreFinale.GetSaturation() - coloreIniziale.GetSaturation(); // differenza da coprire
            //  spanSaturation = 0;
            //    //spanLuminance = coloreFinale.GetBrightness() - coloreIniziale.GetBrightness(); // differenza da coprire
            //    spanLuminance = 0;
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
                currentInjection.InjectionPositionX = currentNearestReferencePosition.X;
                currentInjection.InjectionPositionY = currentNearestReferencePosition.Y;
            }
            // clears the GraphicsView that will be redrawn by the Draw method
            // the system will launch the Draw() method to redraw

            //// dal branch colori
            //            // cambia colore dal colore iniziale a quello finale
            //            colHSL.Hue = (int)(initialColor.GetHue() + spanHue * (timeTotalSeconds * 60 - timeLeftSeconds) / (timeTotalSeconds * 60));
            //            colHSL.Saturation = initialColor.GetSaturation() + spanSaturation * (timeTotalSeconds * 60 - timeLeftSeconds) / (timeTotalSeconds * 60);
            //            colHSL.Luminance = initialColor.GetBrightness() + spanLuminance * (timeTotalSeconds * 60 - timeLeftSeconds) / timeTotalSeconds;
            //            ColorHelper.ColorConverter.HSL2RGB(colHSL, colRGB);
            //            currentColor = colRGB.Color;

            //             // se in modalità "editing", sposta il cerchio in quella posizione
            //            cerchioD.LeftCerchio = (float)x;
            //            cerchioD.TopCerchio = (float)y;
            //            // rende non valido tutto quello che c'è nel cerchiGraphicsView.
            //            // Pertanto il sistema deve lanciare il metodo Draw() per ridisegnare 
            //            // TUTTA la grafica che c'era
            //// FINE dal branch colori
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
        currentInjection.InjectionPositionX = currentNearestReferencePosition.X;
        currentInjection.InjectionPositionY = currentNearestReferencePosition.Y;
        // we pass the coordinates of the point to the calling Page, through the injection object
        currentInjection.Zone = Common.ZoneOfPosition.Back;

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