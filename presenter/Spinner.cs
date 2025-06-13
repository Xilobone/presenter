using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

public class Spinner
{
    //spinner properties
    float rotation = 0;
    int radius = 300;
    float rotationSpeed = 0.05f; //rotations per second
    int segments = 10;
    
    //holds rgb values
    byte[] r;
    byte[] g;
    byte[] b;

    Random random;
    Canvas canvas;
    DispatcherTimer? timer;
    public Spinner(Canvas canvas)
    {
        this.canvas = canvas;

        //create random colors
        random = new Random();
        r = new byte[segments];
        g = new byte[segments];
        b = new byte[segments];

        random.NextBytes(r);
        random.NextBytes(g);
        random.NextBytes(b);
    }

    public void Start()
    {
        if (timer == null)
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000f / 60f) // 60 FPS
            };
            timer.Tick += Update;
            timer.Start();
        }
    }
    public void Update(object? sender, EventArgs e)
    {
        rotation += rotationSpeed * (float)(Math.PI * 2) / 60f;
        canvas.Children.Clear();
        CreateSpinner();
        CreateLabels();
    }

    void CreateSpinner()
    {
        Point center = new Point(canvas.Width / 2, canvas.Height / 2);

        for (int i = 0; i < segments; i++)
        {
            double angle = (i * 2 * Math.PI) / segments;
            double angle2 = ((i + 1) * 2 * Math.PI) / segments;

            Point point1 = new Point(center.X + radius * Math.Cos(angle + rotation), center.Y + radius * Math.Sin(angle + rotation));
            Point point2 = new Point(center.X + radius * Math.Cos(angle2 + rotation), center.Y + radius * Math.Sin(angle2 + rotation));

            PathFigure figure = new PathFigure
            {
                StartPoint = center,
                IsClosed = true,
                IsFilled = true
            };

            figure.Segments.Add(new LineSegment(point1, true));
            figure.Segments.Add(new ArcSegment
            {
                Point = point2,
                Size = new Size(radius, radius),
                IsLargeArc = false,
                SweepDirection = SweepDirection.Clockwise
            });

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            Path path = new Path
            {
                Fill = new SolidColorBrush(Color.FromRgb(r[i], g[i], b[i])),
                StrokeThickness = 3,
                Data = geometry
            };

            canvas.Children.Add(path);
        }
    }
        void CreateLabels()
        {
        Point center = new Point(canvas.Width / 2, canvas.Height / 2);

        for (int i = 0; i < segments; i++)
            {
                double angle = (i * 2 * Math.PI) / segments;
                Point labelPosition = new Point(center.X + (radius + 20) * Math.Cos(angle + rotation), center.Y + (radius + 20) * Math.Sin(angle + rotation));
                TextBlock label = new TextBlock
                {
                    Text = $"{i + 1}",
                    Foreground = Brushes.White,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(label, labelPosition.X - label.ActualWidth / 2);
                Canvas.SetTop(label, labelPosition.Y - label.ActualHeight / 2);
                canvas.Children.Add(label);
            }
        }
    
}
