using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Media.Media3D;

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
            UpdateCurrentMonthYear();
            LoadSelectedDate(DateTime.Today);
        }

        private void UpdateCurrentMonthYear()
        {
            if (mainCalendar.DisplayDate != null)
            {
                currentMonthYear.Text = mainCalendar.DisplayDate.ToString("MMMM yyyy");

                // Update year buttons
                var year = mainCalendar.DisplayDate.Year;
                var yearStackPanel = VisualTreeHelper.GetParent(currentYearButton) as StackPanel;
                if (yearStackPanel != null)
                {
                    foreach (Button btn in yearStackPanel.Children)
                    {
                        if (btn.Tag != null && int.TryParse(btn.Tag.ToString(), out int btnYear))
                        {
                            btn.FontSize = btnYear == year ? 24 : 14;
                            btn.Foreground = btnYear == year ?
                                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F6F95")) :
                                new SolidColorBrush(Colors.Black);
                        }
                    }
                }

                // Update month buttons
                var monthWrapPanel = this.FindName("monthWrapPanel") as WrapPanel;
                if (monthWrapPanel != null)
                {
                    foreach (Button btn in monthWrapPanel.Children)
                    {
                        if (btn.Tag != null && int.TryParse(btn.Tag.ToString(), out int btnMonth))
                        {
                            btn.Foreground = btnMonth == mainCalendar.DisplayDate.Month ?
                                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F6F95")) :
                                new SolidColorBrush(Colors.Black);
                        }
                    }
                }
            }
        }

        private void Year_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                if (int.TryParse(btn.Tag.ToString(), out int year))
                {
                    mainCalendar.DisplayDate = new DateTime(year, mainCalendar.DisplayDate.Month, 1);
                    UpdateCurrentMonthYear();
                }
            }
        }

        private void Month_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                if (int.TryParse(btn.Tag.ToString(), out int month))
                {
                    mainCalendar.DisplayDate = new DateTime(mainCalendar.DisplayDate.Year, month, 1);
                    UpdateCurrentMonthYear();
                }
            }
        }

        private void PreviousYear_Click(object sender, RoutedEventArgs e)
        {
            mainCalendar.DisplayDate = mainCalendar.DisplayDate.AddYears(-1);
            UpdateCurrentMonthYear();
        }

        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            mainCalendar.DisplayDate = mainCalendar.DisplayDate.AddYears(1);
            UpdateCurrentMonthYear();
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

                return new AppointmentViewModel
                {
                    AppointmentId = a.AppointmentID,
                    PatientId = a.PatientID_FK,
                    PatientName = patient?.PatientName ?? "Unknown Patient",
                    AppointmentTime = a.AppointmentTime,
                    AppointmentDate = a.AppointmentDate,
                    // Brighter color for upcoming appointments
                    StatusColor = isPast ? "#759DC0" : "#7BD2FF",
                    // Different icon for past vs upcoming
                    StatusIcon = isPast ? "CheckCircle" : "CircleThin",
                    // White text for upcoming appointments
                    TextColor = isPast ? "#000000" : "#FFFFFF"
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

        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is AppointmentViewModel appointment)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the appointment for {appointment.PatientName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Delete the appointment from the database
                        var appointmentToDelete = _appointmentHelper.GetAllData()
                            .FirstOrDefault(a => a.AppointmentID == appointment.AppointmentId);

                        if (appointmentToDelete != null)
                        {
                            _appointmentHelper.Delete(appointmentToDelete);
                            LoadAppointments(); // Refresh the appointments list
                            MessageBox.Show("Appointment deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is AppointmentViewModel appointment)
            {
                try
                {
                    // Get the patient associated with this appointment
                    var patient = _patientHelper.GetAllData()
                        .FirstOrDefault(p => p.PatientID == appointment.PatientId);

                    if (patient != null)
                    {
                        var existingPatientWindow = new ExistPatient();

                        // Set the appointment date and time
                        existingPatientWindow.AppointmentDatePicker.SelectedDate = appointment.AppointmentDate;
                        existingPatientWindow.AppointmentTimeComboBox.Text = appointment.AppointmentTime;

                        // Set patient details
                        existingPatientWindow.PatientNameTextBox.Text = patient.PatientName;
                        existingPatientWindow.PatientIDTextBox.Text = patient.PatientID.ToString();
                        existingPatientWindow.PatientAgeTextBox.Text = (DateTime.Now.Year - patient.DateOfBirth.Year).ToString();

                        // Store the appointment ID for updating
                        existingPatientWindow.AppointmentId = appointment.AppointmentId;

                        // Show the window
                        existingPatientWindow.ShowDialog();

                        // Refresh appointments after edit
                        LoadAppointments();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening edit window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadAppointments()
        {
            if (mainCalendar.SelectedDate.HasValue)
            {
                LoadSelectedDate(mainCalendar.SelectedDate.Value);
            }
        }
    }

    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string AppointmentTime { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }
        public string TextColor { get; set; }
    }

    public class AppointmentItemViewModel
    {
        public string PatientName { get; set; }
        public string AppointmentTime { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }
        public string TextColor { get; set; }
    }
}