using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLib.Shapes;

namespace SimplePaintApp
{
    public partial class MainWindow : Window
    {
        private Shape currentShape;
        private bool isDrawing = false;
        private Point startPoint;
        private DrawingLib.Shapes.Shape drawingShape;
        private enum ShapeType { Line, Rectangle, Ellipse }
        private ShapeType currentShapeType = ShapeType.Line;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            currentShapeType = ShapeType.Line;
        }

        private void btnRectangle_Click(object sender, RoutedEventArgs e)
        {
            currentShapeType = ShapeType.Rectangle;
        }

        private void btnEllipse_Click(object sender, RoutedEventArgs e)
        {
            currentShapeType = ShapeType.Ellipse;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;
            startPoint = e.GetPosition(drawingCanvas);

            switch (currentShapeType)
            {
                case ShapeType.Line:
                    currentShape = new Line
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        X1 = startPoint.X,
                        Y1 = startPoint.Y,
                        X2 = startPoint.X,
                        Y2 = startPoint.Y
                    };
                    drawingShape = new LineShape();
                    break;
                case ShapeType.Rectangle:
                    currentShape = new Rectangle
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };
                    Canvas.SetLeft(currentShape, startPoint.X);
                    Canvas.SetTop(currentShape, startPoint.Y);
                    drawingShape = new RectangleShape();
                    break;
                case ShapeType.Ellipse:
                    currentShape = new Ellipse
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };
                    Canvas.SetLeft(currentShape, startPoint.X);
                    Canvas.SetTop(currentShape, startPoint.Y);
                    drawingShape = new EllipseShape();
                    break;
            }

            drawingCanvas.Children.Add(currentShape);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || currentShape == null)
                return;

            Point currentPoint = e.GetPosition(drawingCanvas);

            switch (currentShapeType)
            {
                case ShapeType.Line:
                    Line line = currentShape as Line;
                    line.X2 = currentPoint.X;
                    line.Y2 = currentPoint.Y;
                    break;
                case ShapeType.Rectangle:
                    Rectangle rect = currentShape as Rectangle;
                    double x = Math.Min(currentPoint.X, startPoint.X);
                    double y = Math.Min(currentPoint.Y, startPoint.Y);
                    double w = Math.Abs(currentPoint.X - startPoint.X);
                    double h = Math.Abs(currentPoint.Y - startPoint.Y);
                    Canvas.SetLeft(rect, x);
                    Canvas.SetTop(rect, y);
                    rect.Width = w;
                    rect.Height = h;
                    break;
                case ShapeType.Ellipse:
                    Ellipse ellipse = currentShape as Ellipse;
                    x = Math.Min(currentPoint.X, startPoint.X);
                    y = Math.Min(currentPoint.Y, startPoint.Y);
                    w = Math.Abs(currentPoint.X - startPoint.X);
                    h = Math.Abs(currentPoint.Y - startPoint.Y);
                    Canvas.SetLeft(ellipse, x);
                    Canvas.SetTop(ellipse, y);
                    ellipse.Width = w;
                    ellipse.Height = h;
                    break;
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
            currentShape = null;
        }
    }
}
