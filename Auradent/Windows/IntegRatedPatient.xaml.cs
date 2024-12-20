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
    /// Interaction logic for IntegRatedPatient.xaml
    /// </summary>
    public partial class IntegRatedPatient : Window
    {
        public IntegRatedPatient()
        {
            InitializeComponent();
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
    }
}