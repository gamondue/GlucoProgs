namespace GlucoMan.Maui
{
    internal class CirclesDrawable : IDrawable
    {
        internal float LeftCerchio { get; set; } = 0;
        internal float TopCerchio { get; set; } = 0;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // colore con cui disegnerò la linea del cerchio
            canvas.StrokeColor = Colors.Red;
            // dimensione della linea che disegnerò
            canvas.StrokeSize = 4;
            // colore di riempimento delle forme che disegnerò
            canvas.FillColor = Colors.Blue; // NON MI FUNZIONA, MA DOVREBBE!!

            // disegno del cerchio
            canvas.DrawEllipse(LeftCerchio, TopCerchio, 20, 20);
            //canvas.DrawEllipse(LeftCerchio, TopCerchio, 4, 4);
        }
    }
}
