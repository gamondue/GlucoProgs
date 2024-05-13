using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;

namespace GlucoMan.Maui;

public partial class BackPicturePage : ContentPage
{
    CirclesDrawable cerchioD;
    public BackPicturePage()
    {
        InitializeComponent();

        // Istanziazione dell'oggetto che disegnerà il (i) cerchio
        cerchioD = new CirclesDrawable();
        // collegamento del Drawable al GraphicsView
        cerchiGraphicsView.Drawable = cerchioD;
        cerchiGraphicsView.Background = Color.FromRgba(255, 255, 0, 100);
        var children = cerchiGraphicsView.GetChildElements(new Point(100, 100));
        //cerchiGraphicsView.Width = Immagine.Width;

        // "reset" del GraphicsView, che rende necessario il lancio del metodo Draw (prima prova di disegno cerchio) 
        cerchiGraphicsView.Invalidate();
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

            Color initialColor = Color.FromArgb("00FF00");
            //Color coloreIniziale = Color.Green;
            Color finalColor = Color.FromArgb("FF0000");
            Color currentColor;

            float spanHue;          // differenza di tinta da coprire
            float spanSaturation;   // differenza di saturazione da coprire
            float spanLuminance;    // differenza di saturazione da coprire
            spanHue = finalColor.GetHue() - initialColor.GetHue(); // differenza di tinta da coprire
            //spanSaturation = coloreFinale.GetSaturation() - coloreIniziale.GetSaturation(); // differenza da coprire
            spanSaturation = 0;
            //spanLuminance = coloreFinale.GetBrightness() - coloreIniziale.GetBrightness(); // differenza da coprire
            spanLuminance = 0;


            // se in modalità "non editing", memorizza l'istante della puntura
            // in seguito cambio il colore del cerchio in base alla differenza di tempo 
            // fra DateTime.Now e l'istante memorizzato

            // cambio di colore con ColorConverter, si passa da rosso a verde
            // in base alla differenza di tempo fra DateTime.Now e l'istante memorizzato
            // inoltre, si passa da un colore all'altro in 60 secondi
            // il colore cambia con una legge lineare da rosso a verde in 60 secondi

            ColorConverter.RGB colRGB = new ColorHelper.RGB();
            ColorHelper.HSL colHSL = new ColorHelper.HSL();

            // cambia colore dal colore iniziale a quello finale
            colHSL.Hue = (int)(initialColor.GetHue() + spanHue * (timeTotalSeconds * 60 - timeLeftSeconds) / (timeTotalSeconds * 60));
            colHSL.Saturation = initialColor.GetSaturation() + spanSaturation * (timeTotalSeconds * 60 - timeLeftSeconds) / (timeTotalSeconds * 60);
            colHSL.Luminance = initialColor.GetBrightness() + spanLuminance * (timeTotalSeconds * 60 - timeLeftSeconds) / timeTotalSeconds;
            ColorHelper.ColorConverter.HSL2RGB(colHSL, colRGB);
            currentColor = colRGB.Color;
 

             // se in modalità "editing", sposta il cerchio in quella posizione
            cerchioD.LeftCerchio = (float)x;
            cerchioD.TopCerchio = (float)y;
            // rende non valido tutto quello che c'è nel cerchiGraphicsView.
            // Pertanto il sistema deve lanciare il metodo Draw() per ridisegnare 
            // TUTTA la grafica che c'era
            this.cerchiGraphicsView.Invalidate();
        }
    }
}