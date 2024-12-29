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

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for ToothDetailWindow.xaml
    /// </summary>
    public partial class ToothDetailWindow : Window
    {
        public ToothDetailWindow()
        {
            InitializeComponent();
        }


        // Handle the mouse click on the canvas to place a marker
        private void ToothCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Get the position of the click relative to the Canvas
            var position = e.GetPosition(ToothCanvas);

            // Create a new ellipse (marker) at the clicked location
            Ellipse marker = new Ellipse
            {
                Width = 10,  // Size of the marker
                Height = 10,
                Fill = Brushes.Red,  // Color of the marker
                Stroke = Brushes.Black,  // Stroke color of the marker
                StrokeThickness = 1
            };

            // Set the position of the marker (anchor it to the click point)
            Canvas.SetLeft(marker, position.X - marker.Width / 2);  // Center marker on the click point
            Canvas.SetTop(marker, position.Y - marker.Height / 2);

            // Add the marker to the Canvas
            ToothCanvas.Children.Add(marker);
        }

        // Save notes button click
        private void SaveNotes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Notes saved: {NotesBox.Text}");
        }

    }
}
