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
using Auradent.View.Usercontrols;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for Nurse_s_dashboard.xaml
    /// </summary>
    public partial class Nurse_s_dashboard : Window
    {
        private readonly IdataHelper<Auradent.core.Patient> _patientHelper;
        private readonly IdataHelper<Appointment> _appointmentHelper;

        public Nurse_s_dashboard()
        {
            InitializeComponent();

            var services = ((App)Application.Current).Services;
            _appointmentHelper = services.GetService<IdataHelper<Appointment>>();
            _patientHelper = services.GetService<IdataHelper<Auradent.core.Patient>>();

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

                bool isCompleted = a.AppointmentStatus == "Completed";

                return new AppointmentItemViewModel
                {
                    AppointmentID = a.AppointmentID,
                    PatientID = a.PatientID_FK,
                    PatientName = patient?.PatientName ?? "Unknown Patient",
                    AppointmentTime = a.AppointmentTime,
                    StatusColor = isCompleted ? "#4CAF50" : (isPast ? "#759DC0" : "#7BD2FF"),
                    StatusIcon = isCompleted ? "CheckCircle" : (isPast ? "CheckCircle" : "CircleThin"),
                    TextColor = isPast ? "#000000" : "#FFFFFF",
                    IsCompleted = isCompleted
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

        private void Refresh_btn(object sender, RoutedEventArgs e)
        {
            // Simply reload the current date's data
            if (mainCalendar.SelectedDate.HasValue)
            {
                LoadSelectedDate(mainCalendar.SelectedDate.Value);
            }
        }

        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var appointmentItem = button.DataContext as AppointmentItemViewModel;

            if (appointmentItem != null)
            {
                var patient = _patientHelper.GetAllData().FirstOrDefault(p => p.PatientID == appointmentItem.PatientID);
                if (patient != null)
                {
                    var existPatient = new ExistPatient(appointmentItem.AppointmentID);
                    existPatient.Show();
                    LoadSelectedDate(mainCalendar.SelectedDate ?? DateTime.Today);
                }
            }
        }

        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var appointmentItem = button.DataContext as AppointmentItemViewModel;

            if (appointmentItem != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this appointment?", "Delete Appointment",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var appointment = _appointmentHelper.GetAllData()
                        .FirstOrDefault(a => a.AppointmentID == appointmentItem.AppointmentID);

                    if (appointment != null)
                    {
                        _appointmentHelper.Delete(appointment);
                        LoadSelectedDate(mainCalendar.SelectedDate ?? DateTime.Today);
                    }
                }
            }
        }

        private void Item_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Auradent.View.Usercontrols.Item item)
            {
                var appointmentId = item.AppointmentID;
                var isChecked = item.IsChecked;

                var appointment = _appointmentHelper.GetAllData()
                    .FirstOrDefault(a => a.AppointmentID == appointmentId);

                if (appointment != null)
                {
                    appointment.AppointmentStatus = isChecked ? "Completed" : "Pending";
                    _appointmentHelper.Update(appointment);
                    LoadSelectedDate(mainCalendar.SelectedDate ?? DateTime.Today);
                }
            }
        }
    }

    public class AppointmentItemViewModel
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string AppointmentTime { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }
        public string TextColor { get; set; }
        public bool IsCompleted { get; set; }
    }
}