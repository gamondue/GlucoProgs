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

            cerchioD.LeftCerchio = (float)x;
            cerchioD.TopCerchio = (float)y;
            // rende non valido tutto quello che c'è nel cerchiGraphicsView.
            // Pertanto il sistema deve lanciare il metodo Draw() per ridisegnare 
            // TUTTA la grafica che c'era
            this.cerchiGraphicsView.Invalidate();
        }
    }
}