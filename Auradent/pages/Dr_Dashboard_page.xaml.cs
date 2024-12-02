using Auradent.View.Usercontrols;
using Auradent.Windows;
using Auradent.Windows.Clinic_Finance;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Auradent.pages
{
    /// <summary>
    /// Interaction logic for Dr_Dashboard_page.xaml
    /// </summary>
    public partial class Dr_Dashboard_page : Page
    {
        public Dr_Dashboard_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RenderPages.Children.Clear();
            RenderPages.Children.Add(new Dashboard_for_dr());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clinic_Finance newWindow = new Clinic_Finance
            {
                Title = "New Window",
                WindowState = WindowState.Normal // Optional: Open the window maximized
            };

            // Show the new window
            newWindow.Show();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            MainWindow newmainWindow = new MainWindow
            {
                Title = "Finance",
                WindowState = WindowState.Maximized // Optional: Open the window maximized
            };

            // Show the new window
            newmainWindow.Show();
            Window.GetWindow(this)?.Close();
        }

        private void appointment(object sender, RoutedEventArgs e)
        {
            MedicalRecord newWindow = new MedicalRecord
            {
                Title = "Medical Record",
                WindowState = WindowState.Normal // Optional: Open the window maximized
            };

            // Show the new window
            newWindow.Show();
        }
    }
}
