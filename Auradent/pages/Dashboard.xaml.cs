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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private readonly Dictionary<int, string> appointmentInfo;
        public Dashboard()
        {
            InitializeComponent();
            appointmentInfo = new Dictionary<int, string>
            {
                { 0, "John Doe - Sun, 2:30 PM (Type of appointment: Follow-up)" },
                { 1, "Jane Smith - Mon, 3:00 PM (Type of appointment: Urgent)" },
                { 2, "Mike Johnson - Tue, 1:00 PM (Type of appointment: Routine)" },
                { 3, "Alice Brown - Wed, 4:15 PM (Type of appointment: Follow-up)" },
                { 4, "Bob White - Thu, 2:30 PM (Type of appointment: Initial Consultation)" },
                { 5, "Carol Black - Fri, 5:00 PM (Type of appointment: Urgent)" },
                { 6, "Sam Green - Sat, 11:00 AM (Type of appointment: Routine)" }
            };

        }

        private void menubar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Check if sender is a button and retrieve its tag as index
            if (sender is Button clickedButton && int.TryParse(clickedButton.Tag.ToString(), out int index))
            {
                // Update AppointmentInfo TextBlock with the appointment details
                AppointmentInfo.Text = appointmentInfo.ContainsKey(index) ? appointmentInfo[index] : "No appointment data available";
            }
        }
    }
}
