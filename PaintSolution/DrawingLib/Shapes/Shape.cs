using System.Windows.Media;
using System.Windows.Input;
using System.Windows;

namespace DrawingLib.Shapes
{
    public abstract class Shape
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Brush Stroke { get; set; } = Brushes.Black;
        public double StrokeThickness { get; set; } = 2.0;

        public abstract void Draw(DrawingContext dc);
    }
}
