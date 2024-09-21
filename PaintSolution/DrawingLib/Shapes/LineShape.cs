using System.Windows.Media;

namespace DrawingLib.Shapes
{
    public class LineShape : Shape
    {
        public override void Draw(DrawingContext dc)
        {
            dc.DrawLine(new Pen(Stroke, StrokeThickness), StartPoint, EndPoint);
        }
    }
}
