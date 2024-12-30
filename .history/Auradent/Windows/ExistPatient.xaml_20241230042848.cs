using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Windows
{
    public partial class ExistPatient : Window
    {
        private readonly IdataHelper<Patient> _patientHelper;
        private readonly IdataHelper<Appointment> _appointmentHelper;
        private ObservableCollection<TimeSlot> _timeSlots;
        private ObservableCollection<string> _filteredPatients;

        public ExistPatient()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _patientHelper = services.GetService<IdataHelper<Patient>>();
            _appointmentHelper = services.GetService<IdataHelper<Appointment>>();
            _filteredPatients = new ObservableCollection<string>();

            InitializeTimeSlots();
            LoadPatients();
        }

        private void SearchComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string searchText = SearchComboBox.Text + e.Text;
            UpdateFilteredPatients(searchText);
        }

        private void UpdateFilteredPatients(string searchText)
        {
            _filteredPatients.Clear();
            var patients = _patientHelper.GetAllData();

            var filteredResults = patients
                .Where(p => p.PatientName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.PatientName)
                .ToList();

            foreach (var name in filteredResults)
            {
                _filteredPatients.Add(name);
            }

            SearchComboBox.ItemsSource = _filteredPatients;
            SearchComboBox.IsDropDownOpen = true;
        }

        private void AppointmentDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            // Set display date start to today
            AppointmentDatePicker.DisplayDateStart = DateTime.Today;
            AppointmentDatePicker.SelectedDate = DateTime.Today;

            // Add blackout dates for past dates
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1));
            AppointmentDatePicker.BlackoutDates.Add(blackoutRange);
        }

        private void InitializeTimeSlots()
        {
            _timeSlots = new ObservableCollection<TimeSlot>();
            var startTime = new TimeSpan(14, 0, 0); // 14:00
            var endTime = new TimeSpan(22, 0, 0);   // 22:00
            var interval = TimeSpan.FromMinutes(30);

            for (var time = startTime; time <= endTime; time += interval)
            {
                _timeSlots.Add(new TimeSlot { Time = time.ToString(@"hh\:mm"), IsBooked = false });
            }

            AppointmentTimeComboBox.ItemsSource = _timeSlots;
        }

        private void LoadPatients()
        {
            var patients = _patientHelper.GetAllData();
            _filteredPatients = new ObservableCollection<string>(patients.Select(p => p.PatientName));
            SearchComboBox.ItemsSource = _filteredPatients;
        }

        private void AppointmentDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppointmentDatePicker.SelectedDate.HasValue)
            {
                UpdateAvailableTimeSlots(AppointmentDatePicker.SelectedDate.Value);
            }
        }

        private void UpdateAvailableTimeSlots(DateTime selectedDate)
        {
            // Reset all time slots
            foreach (var slot in _timeSlots)
            {
                slot.IsBooked = false;
            }

            // Get all appointments for the selected date
            var appointments = _appointmentHelper.GetAllData()
                .Where(a => a.AppointmentDate.Date == selectedDate.Date)
                .ToList();

            // Mark booked time slots
            foreach (var appointment in appointments)
            {
                var timeSlot = _timeSlots.FirstOrDefault(t => t.Time == appointment.AppointmentTime);
                if (timeSlot != null)
                {
                    timeSlot.IsBooked = true;
                }
            }

            // Refresh the ComboBox
            AppointmentTimeComboBox.Items.Refresh();
        }

        private void SearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchComboBox.SelectedItem != null)
            {
                var selectedPatient = _patientHelper.GetAllData()
                    .FirstOrDefault(p => p.PatientName == SearchComboBox.SelectedItem.ToString());

                if (selectedPatient != null)
                {
                    PatientNameTextBox.Text = selectedPatient.PatientName;
                    PatientIDTextBox.Text = selectedPatient.PatientID.ToString();
                    PatientAgeTextBox.Text = (DateTime.Now.Year - selectedPatient.DateOfBirth.Year).ToString();
                }
            }
        }

        private void Save_btn(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var selectedPatient = _patientHelper.GetAllData()
                    .FirstOrDefault(p => p.PatientName == PatientNameTextBox.Text);

                if (selectedPatient != null && AppointmentDatePicker.SelectedDate.HasValue)
                {
                    var appointment = new Appointment
                    {
                        PatientID_FK = selectedPatient.PatientID,
                        AppointmentDate = AppointmentDatePicker.SelectedDate.Value,
                        AppointmentTime = ((TimeSlot)AppointmentTimeComboBox.SelectedItem).Time
                    };

                    _appointmentHelper.Add(appointment);
                    MessageBox.Show("Appointment saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(PatientNameTextBox.Text))
            {
                MessageBox.Show("Please select a patient.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!AppointmentDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select an appointment date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (AppointmentTimeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select an appointment time.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }

    public class TimeSlot : System.ComponentModel.INotifyPropertyChanged
    {
        private string _time;
        private bool _isBooked;

        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public bool IsBooked
        {
            get => _isBooked;
            set
            {
                _isBooked = value;
                OnPropertyChanged(nameof(IsBooked));
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
