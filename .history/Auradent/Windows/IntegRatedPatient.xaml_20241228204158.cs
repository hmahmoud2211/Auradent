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
using System.ComponentModel;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for IntegRatedPatient.xaml
    /// </summary>
    public partial class IntegRatedPatient : Window, INotifyPropertyChanged
    {
        private string _lastVisitDateTime1;
        private string _lastVisitDateTime2;
        private string _lastVisitDateTime3;

        public event PropertyChangedEventHandler PropertyChanged;

        public string LastVisitDateTime1
        {
            get => _lastVisitDateTime1;
            set
            {
                _lastVisitDateTime1 = value;
                OnPropertyChanged(nameof(LastVisitDateTime1));
            }
        }

        public string LastVisitDateTime2
        {
            get => _lastVisitDateTime2;
            set
            {
                _lastVisitDateTime2 = value;
                OnPropertyChanged(nameof(LastVisitDateTime2));
            }
        }

        public string LastVisitDateTime3
        {
            get => _lastVisitDateTime3;
            set
            {
                _lastVisitDateTime3 = value;
                OnPropertyChanged(nameof(LastVisitDateTime3));
            }
        }

        public IntegRatedPatient()
        {
            InitializeComponent();
            DataContext = this;
            LoadRecentPatients();
        }

        private void LoadRecentPatients()
        {
            // This is where you would normally fetch data from your database
            // For demonstration, using current time with offsets
            DateTime now = DateTime.Now;
            
            LastVisitDateTime1 = now.AddHours(-1).ToString("MM/dd/yyyy HH:mm");
            LastVisitDateTime2 = now.AddHours(-3).ToString("MM/dd/yyyy HH:mm");
            LastVisitDateTime3 = now.AddHours(-5).ToString("MM/dd/yyyy HH:mm");
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Medical_rec_Btn(object sender, RoutedEventArgs e)
        {
            MedicalRecord secondWindow = new MedicalRecord();
            secondWindow.Show();
        }

        private void Home_btn(object sender, RoutedEventArgs e)
        {
            Window pageWindow = new Window
            {
                Title = "Page Window",
                WindowState = WindowState.Maximized
            };
            Frame frame = new Frame();
            frame.Navigate(new Uri("\\pages\\Dr_Dashboard_page.xaml", UriKind.Relative));
            pageWindow.Content = frame;
            pageWindow.Show();
            this.Close();
        }

        private void signout_btn(object sender, RoutedEventArgs e)
        {
            MainWindow secondWindow = new MainWindow();
            secondWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the Rectangle dynamically
            var redRectangle = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Red
            };

            // Set position on the Canvas
            Canvas.SetLeft(redRectangle, 100);  // X position
            Canvas.SetTop(redRectangle, 100);   // Y position

            // Add the rectangle to the Canvas

        }

        private void Tooth_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button toothButton)
            {
                // Open the detailed tooth window
                ToothDetailWindow detailWindow = new ToothDetailWindow();
                detailWindow.ShowDialog();
                StatusText.Text = $"Viewing details for Tooth {toothButton.Content}.";
            }
        }


    }
}