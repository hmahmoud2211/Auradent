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
    /// Interaction logic for chatbot.xaml
    /// </summary>
    public partial class chatbot : Window
    {
        public chatbot()
        {
            InitializeComponent();
        }
        // Event handler for Dashboard button click
        private void Dashboard_Click(object sender, RoutedEventArgs e, string DashboardPage)
        {
            NavigateToPage(DashboardPage);
        }

        // Event handler for Patients button click
        private void Patients_Click(object sender, RoutedEventArgs e, string PatientsPage)
        {
            NavigateToPage(PatientsPage);
        }

        // Event handler for Appointments button click
        private void Appointments_Click(object sender, RoutedEventArgs e, string AppointmentsPage)
        {
            NavigateToPage(AppointmentsPage);
        }

        // Event handler for Messages button click
        private void Messages_Click(object sender, RoutedEventArgs e, string MessagesPage)
        {
            NavigateToPage(MessagesPage);
        }

        // Event handler for Revenue button click
        private void Revenue_Click(object sender, RoutedEventArgs e, string RevenuePage)
        {
            NavigateToPage(RevenuePage);
        }

        // Event handler for Settings button click
        private void Settings_Click(object sender, RoutedEventArgs e, string SettingsPage)
        {
            NavigateToPage(SettingsPage);
        }

        // Event handler for Logout button click
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // You can add your logout logic here
            MessageBox.Show("Logged out successfully.");
        }

        // Helper method to navigate to a page
        private void NavigateToPage(string pageName)
        {
            // Clear the existing content
            MainContentFrame.Content = null;

            // Based on the pageName, load the respective page

        }
    }
}
