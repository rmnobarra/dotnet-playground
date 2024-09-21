using System.Windows;
using System.Windows.Media;

namespace DrawingLib.Shapes
{
    public class EllipseShape : Shape
    {
        public override void Draw(DrawingContext dc)
        {
            Rect rect = new Rect(StartPoint, EndPoint);
            dc.DrawEllipse(null, new Pen(Stroke, StrokeThickness), rect.Location + rect.Size / 2, rect.Width / 2, rect.Height / 2);
        }
    }
}
