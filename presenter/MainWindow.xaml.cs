//using System.Drawing;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace presenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        float rotation = 0;
        float rotationSpeed = 0.05f; //rotations per second

        int segments = 10;
        Random random;

        byte[] r;
        byte[] g;
        byte[] b;
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += OnRendering;
            
            random = new Random();
            r = new byte[segments];
            g = new byte[segments];
            b = new byte[segments];

            random.NextBytes(r);
            random.NextBytes(g);
            random.NextBytes(b);

            CreateShape();

            StartTimer();
        }

        void StartTimer()
        {   
            float updateInterval = 1000f / 60f;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(updateInterval)
            };
            timer.Tick += (sender, e) =>
            {
                rotation += rotationSpeed * (float)(Math.PI * 2) / 60f;
            };
            timer.Start();
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            canvas.Children.Clear();
            CreateShape();

        }

        void CreateShape()
        {
            int radius = 300;
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


    }
}