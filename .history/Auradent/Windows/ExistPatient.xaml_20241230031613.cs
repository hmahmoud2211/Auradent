using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Auradent.Windows
{
    public partial class ExistPatient : Window
    {
        private IdataHelper<Patient> dataHelperPatient;
        private IdataHelper<Appointment> dataHelperAppointment;
        private IdataHelper<Finance> dataHelperFinance;
        private List<string> allNames;
        public int? AppointmentID { get; set; }

        public ExistPatient()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            if (services == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            dataHelperPatient = services.GetService<IdataHelper<Patient>>() ?? throw new InvalidOperationException("Data helper service is not available.");
            dataHelperAppointment = services.GetService<IdataHelper<Appointment>>() ?? throw new InvalidOperationException("Data helper service is not available.");
            dataHelperFinance = services.GetService<IdataHelper<Finance>>() ?? throw new InvalidOperationException("Data helper service is not available.");

            // Set minimum date to today
            AppointmentDatePicker.DisplayDateStart = DateTime.Today;

            // Initialize available time slots
            InitializeTimeSlots();

            // Handle date selection to update time slots
            AppointmentDatePicker.SelectedDateChanged += AppointmentDatePicker_SelectedDateChanged;

            // Load patient names
            allNames = dataHelperPatient.GetAllData()
                .Select(x => $"{x.PatientName} ({x.PatientID}, {x.PatientPhone})")
                .ToList();
        }

        private void InitializeTimeSlots()
        {
            var timeSlots = new List<string>();
            DateTime currentTime = DateTime.Now;
            DateTime startTime = DateTime.Today.AddHours(9); // Clinic opens at 9 AM
            DateTime endTime = DateTime.Today.AddHours(17);  // Clinic closes at 5 PM

            // If it's today, start from the next available hour
            if (AppointmentDatePicker.SelectedDate == DateTime.Today)
            {
                startTime = DateTime.Now.AddHours(1).Date.AddHours(DateTime.Now.AddHours(1).Hour);
                if (startTime.Hour < 9)
                    startTime = DateTime.Today.AddHours(9);
            }

            // Generate time slots in 30-minute intervals
            while (startTime <= endTime)
            {
                timeSlots.Add(startTime.ToString("HH:mm"));
                startTime = startTime.AddMinutes(30);
            }

            // Remove booked time slots
            if (AppointmentDatePicker.SelectedDate.HasValue)
            {
                var bookedSlots = dataHelperAppointment.GetAllData()
                    .Where(a => a.AppointmentDate.Date == AppointmentDatePicker.SelectedDate.Value.Date)
                    .Select(a => a.AppointmentTime)
                    .ToList();

                timeSlots.RemoveAll(t => bookedSlots.Contains(t));
            }

            AppointmentTimeComboBox.ItemsSource = timeSlots;
        }

        private void AppointmentDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppointmentDatePicker.SelectedDate.HasValue)
            {
                InitializeTimeSlots();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        // Handle PreviewTextInput to filter suggestions
        private void SearchComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string query = comboBox.Text.ToLower() + e.Text.ToLower();  // Combine current text with input text

            // Filter patient names or id or phone number  based on the query
            var suggestions = allNames
                .Where(name => name.ToLower().Contains(query))
                .ToList();

            // Display suggestions (autocomplete logic)
            DisplaySuggestions(suggestions);
        }

        private void DisplaySuggestions(List<string> suggestions)
        {
            // Set the filtered list as the ItemsSource for the ComboBox
            SearchComboBox.ItemsSource = suggestions;


            // Optionally, make sure the ComboBox dropdown is open
            SearchComboBox.IsDropDownOpen = true;
        }

        // Event handler for the DatePicker selection
        private void AppointmentDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppointmentDatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = AppointmentDatePicker.SelectedDate.Value;
                Console.WriteLine("Selected Appointment Date: " + selectedDate.ToShortDateString());
            }
        }

        // Event handler for the Appointment Time ComboBox
        private void AppointmentTimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string selectedTime = comboBox.SelectedItem.ToString();
            Console.WriteLine("Selected Appointment Time: " + selectedTime);
        }

        private void SearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // When a suggestion is selected, display it in the ComboBox
            ComboBox comboBox = sender as ComboBox;
            string selectedItem = comboBox.SelectedItem as string;
            if (selectedItem != null)
            {
                Console.WriteLine("Selected Name: " + selectedItem);
            }
            // split every part of selected item to get the id and phone number and name 
            var parts = selectedItem.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string name = parts[0].Trim();
                int id = int.Parse(parts[1].Trim());
                string phone = parts[2].Trim();

                var patient_data = dataHelperPatient.Search(name).FirstOrDefault(p => p.PatientID == id && p.PatientPhone == phone);
                if (patient_data != null)
                {
                    // Display the patient data in the form
                    PatientNameTextBox.Text = patient_data.PatientName;
                    PatientIDTextBox.Text = patient_data.PatientID.ToString();
                    PatientAgeTextBox.Text = (DateTime.Now.Year - patient_data.DateOfBirth.Year).ToString();
                }
            }
        }


        private void Save_btn(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var appointment = AppointmentID.HasValue
                    ? dataHelperAppointment.GetAllData().FirstOrDefault(a => a.AppointmentID == AppointmentID.Value)
                    : new Appointment();

                if (appointment == null)
                {
                    MessageBox.Show("Could not find the appointment to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Update appointment details
                appointment.PatientID_FK = int.Parse(PatientIDTextBox.Text);
                appointment.AppointmentDate = AppointmentDatePicker.SelectedDate ?? DateTime.Now;
                appointment.AppointmentTime = AppointmentTimeComboBox.Text;

                if (AppointmentID.HasValue)
                {
                    // Update existing appointment
                    dataHelperAppointment.Update(appointment);
                    MessageBox.Show("Appointment updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Add new appointment
                    dataHelperAppointment.Add(appointment);
                    MessageBox.Show("Appointment added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (AppointmentDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select an appointment date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(AppointmentTimeComboBox.Text))
            {
                MessageBox.Show("Please select an appointment time.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (AppointmentDatePicker.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Cannot schedule appointments in the past.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
