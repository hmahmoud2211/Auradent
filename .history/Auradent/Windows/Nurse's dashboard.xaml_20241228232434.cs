using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for Nurse_s_dashboard.xaml
    /// </summary>
    public partial class Nurse_s_dashboard : Window
    {
        private readonly IdataHelper<Appointment> _appointmentHelper;
        private readonly IdataHelper<Patient> _patientHelper;

        public Nurse_s_dashboard()
        {
            InitializeComponent();

            var services = ((App)Application.Current).Services;
            _appointmentHelper = services.GetService<IdataHelper<Appointment>>();
            _patientHelper = services.GetService<IdataHelper<Patient>>();

            // Set calendar to today's date
            mainCalendar.SelectedDate = DateTime.Today;
            LoadSelectedDate(DateTime.Today);
        }

        private void LoadSelectedDate(DateTime date)
        {
            // Update date display
            selectedDayText.Text = date.Day.ToString();
            selectedMonthText.Text = date.ToString("MMMM");
            selectedDayOfWeekText.Text = date.ToString("dddd");

            // Get appointments for selected date
            var appointments = _appointmentHelper.GetAllData()
                .Where(a => a.AppointmentDate.Date == date.Date)
                .OrderBy(a => a.AppointmentTime)
                .ToList();

            // Update appointment count
            appointmentCountText.Text = appointments.Count == 0
                ? "No appointments"
                : $"{appointments.Count} appointment{(appointments.Count > 1 ? "s" : "")}";

            // Get current time for comparison
            var currentTime = DateTime.Now.TimeOfDay;

            // Create appointment view models
            var appointmentItems = appointments.Select(a =>
            {
                var patient = _patientHelper.GetAllData()
                    .FirstOrDefault(p => p.PatientID == a.PatientID_FK);

                // Parse appointment time to compare with current time
                TimeSpan appointmentTime;
                TimeSpan.TryParse(a.AppointmentTime, out appointmentTime);

                // Determine if appointment is past or upcoming
                bool isPast = date.Date < DateTime.Today ||
                    (date.Date == DateTime.Today && appointmentTime < currentTime);

                return new AppointmentItemViewModel
                {
                    PatientName = patient?.PatientName ?? "Unknown Patient",
                    AppointmentTime = a.AppointmentTime,
                    // Brighter color for upcoming appointments
                    StatusColor = isPast ? "#759DC0" : "#7BD2FF",
                    // Different icon for past vs upcoming
                    StatusIcon = isPast ? "CheckCircle" : "CircleThin"
                };
            }).ToList();

            // Update appointments list
            appointmentsList.ItemsSource = appointmentItems;
        }

        private void Calendar_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (mainCalendar.SelectedDate.HasValue)
            {
                LoadSelectedDate(mainCalendar.SelectedDate.Value);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void close_btn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Back_btn(object sender, RoutedEventArgs e)
        {
            newdoctordashboard doctorDashboard = new newdoctordashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Doctor Dashboard"
            };
            doctorDashboard.Show();
            this.Close();
        }

        private void new_patient(object sender, RoutedEventArgs e)
        {
            NewPatient secondWindow = new NewPatient();
            secondWindow.Show();
        }

        private void Existing_btn(object sender, RoutedEventArgs e)
        {
            ExistPatient secondWindow = new ExistPatient();
            secondWindow.Show();
        }
    }

    public class AppointmentItemViewModel
    {
        public string PatientName { get; set; }
        public string AppointmentTime { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }
    }
}