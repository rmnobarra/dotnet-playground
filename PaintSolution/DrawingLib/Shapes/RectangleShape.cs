using System.Windows;
using System.Windows.Media;

namespace DrawingLib.Shapes
{
    public class RectangleShape : Shape
    {
        public override void Draw(DrawingContext dc)
        {
            Rect rect = new Rect(StartPoint, EndPoint);
            dc.DrawRectangle(null, new Pen(Stroke, StrokeThickness), rect);
        }
    }
}
