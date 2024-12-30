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
            // change the next line to make it search by name and phone number and id and only display the name by using function
            allNames = dataHelperPatient.GetAllData()
                .Select(x => $"{x.PatientName} ({x.PatientID}, {x.PatientPhone})")
                .ToList();

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
            Appointment appointment = new Appointment
            {
                AppointmentID = dataHelperAppointment.GetAllData().Count > 0 ? dataHelperAppointment.GetAllData().Max(a => a.AppointmentID) + 1 : 1,
                AppointmentStatus = "Pending",
                AppointmentDate = AppointmentDatePicker.SelectedDate.Value,
                AppointmentTime = AppointmentTimeComboBox.Text,
                PatientID_FK = int.Parse(PatientIDTextBox.Text),
                DoctorandNurseID_FK = 1,
                Fainance_Fk = dataHelperAppointment.GetAllData().Count > 0 ? dataHelperAppointment.GetAllData().Max(a => a.AppointmentID) + 1 : 1
            };
            Finance finance = new Finance
            {
                FinanceId = appointment.Fainance_Fk,
                TotalAmount = 350,
                PaymentMethod = "cash",
                TypeOfPayment = "one time",
            };
            if (!dataHelperFinance.CheckIfIdExists(finance.FinanceId))
            {
                dataHelperFinance.Add(finance);
            }

            if (!dataHelperAppointment.CheckIfIdExists(appointment.AppointmentID))
            {
                dataHelperAppointment.Add(appointment);
                MessageBox.Show("Appointment is Booked", "Booking", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Appointment ID already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
