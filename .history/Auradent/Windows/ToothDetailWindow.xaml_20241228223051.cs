using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for ToothDetailWindow.xaml
    /// </summary>
    public partial class ToothDetailWindow : Window
    {
        private bool isEraserMode = false;

        public ToothDetailWindow()
        {
            InitializeComponent();
        }

        private void ToothCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(ToothCanvas);

            if (isEraserMode)
            {
                // Eraser mode: Remove existing points near the click
                var elementsToRemove = ToothCanvas.Children.OfType<Ellipse>()
                    .Where(ellipse => IsPointNearEllipse(clickPoint, ellipse))
                    .ToList();

                foreach (var element in elementsToRemove)
                {
                    ToothCanvas.Children.Remove(element);
                }
            }
            else
            {
                // Drawing mode: Add new point
                Ellipse point = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Red,
                    Stroke = Brushes.DarkRed,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(point, clickPoint.X - point.Width / 2);
                Canvas.SetTop(point, clickPoint.Y - point.Height / 2);

                ToothCanvas.Children.Add(point);
            }
        }

        private bool IsPointNearEllipse(Point clickPoint, Ellipse ellipse)
        {
            double ellipseX = Canvas.GetLeft(ellipse) + ellipse.Width / 2;
            double ellipseY = Canvas.GetTop(ellipse) + ellipse.Height / 2;
            
            // Calculate distance between click point and ellipse center
            double distance = Math.Sqrt(
                Math.Pow(clickPoint.X - ellipseX, 2) + 
                Math.Pow(clickPoint.Y - ellipseY, 2)
            );

            // Consider points within 15 pixels as "near"
            return distance <= 15;
        }

        private void EraserToggle_Click(object sender, RoutedEventArgs e)
        {
            isEraserMode = (sender as ToggleButton)?.IsChecked ?? false;
            ToothCanvas.Cursor = isEraserMode ? Cursors.Hand : Cursors.Arrow;
        }

        private void SaveNotes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Notes saved: {NotesBox.Text}");
        }
    }
}
