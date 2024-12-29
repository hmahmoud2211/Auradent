using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Windows
{
    public partial class Search : Window
    {
        private readonly IdataHelper<Patient> _patientHelper;
        private ObservableCollection<Member> members;
        private bool sortByName = true; // Default sort by name

        public Search()
        {
            InitializeComponent();
            
            // Get the service provider from the App instance
            var app = Application.Current as App;
            if (app?.Services == null)
            {
                MessageBox.Show("Error initializing services. Please restart the application.");
                Close();
                return;
            }

            // Initialize the patient helper
            _patientHelper = app.Services.GetService<IdataHelper<Patient>>();
            if (_patientHelper == null)
            {
                MessageBox.Show("Error initializing patient data. Please restart the application.");
                Close();
                return;
            }

            LoadPatients();
        }

        private void LoadPatients()
        {
            try
            {
                var converter = new BrushConverter();
                members = new ObservableCollection<Member>();
                var patients = _patientHelper.GetAllData()?.ToList() ?? new List<Patient>();

                // Sort patients based on current sorting preference
                patients = sortByName
                    ? patients.OrderBy(p => p.PatientName).ToList()
                    : patients.OrderByDescending(p => p.PatientID).ToList();

                int number = 1;
                foreach (var patient in patients)
                {
                    if (patient == null) continue;

                    string character = !string.IsNullOrEmpty(patient.PatientName)
                        ? patient.PatientName[0].ToString().ToUpper()
                        : "?";

                    string bgColor = GetColorForIndex(number);

                    members.Add(new Member
                    {
                        Number = number.ToString(),
                        Character = character,
                        BgColor = (Brush)converter.ConvertFromString(bgColor),
                        Name = patient.PatientName ?? "Unknown",
                        Position = patient.PatientID.ToString(),
                        Phone = patient.PatientPhone ?? "N/A"
                    });

                    number++;
                }

                if (membersDataGrid != null)
                {
                    membersDataGrid.ItemsSource = members;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patients: {ex.Message}");
            }
        }

        private string GetColorForIndex(int index)
        {
            string[] colors = new[]
            {
                "#1098AD", // Blue
                "#1E88E5", // Light Blue
                "#FF8F00", // Orange
                "#FF5252", // Red
                "#0CA678", // Green
                "#6741D9", // Purple
                "#FF6D00"  // Dark Orange
            };

            return colors[(index - 1) % colors.Length];
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                sortByName = selectedItem.Content.ToString() == "Sort by Name (A-Z)";
                LoadPatients(); // Reload with new sorting
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterPatients(txtSearch.Text);
        }

        private void FilterPatients(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                membersDataGrid.ItemsSource = members;
                return;
            }

            var filteredMembers = members.Where(m =>
                m.Position.Contains(searchText, StringComparison.OrdinalIgnoreCase) || // Search by ID
                m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||    // Search by Name
                m.Phone.Contains(searchText, StringComparison.OrdinalIgnoreCase)      // Search by Phone
            );

            membersDataGrid.ItemsSource = filteredMembers;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool isMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;
                    isMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    isMaximized = true;
                }
            }
        }

        private void home_btn(object sender, RoutedEventArgs e)
        {
            newdoctordashboard doctorDashboard = new newdoctordashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Doctor Dashboard"
            };
            doctorDashboard.Show();
            this.Close();
        }

        private void membersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (membersDataGrid.SelectedItem is Member selectedMember)
            {
                var patient = _patientHelper.GetAllData()
                    .FirstOrDefault(p => p.PatientID.ToString() == selectedMember.Position);

                if (patient != null)
                {
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                    {
                        Title = "Patient Details - " + patient.PatientName,
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                    this.Close();
                }
            }
        }
    }

    public class Member
    {
        public string Number { get; set; }
        public string Character { get; set; }
        public Brush BgColor { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
    }
}