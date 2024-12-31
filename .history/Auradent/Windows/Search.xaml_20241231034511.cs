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
        private string currentSearchType = "ID"; // Default search type

        public Search()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _patientHelper = services.GetService<IdataHelper<Patient>>();

            LoadPatients();
        }

        private void LoadPatients()
        {
            var converter = new BrushConverter();
            members = new ObservableCollection<Member>();
            var patients = _patientHelper.GetAllData().OrderBy(p => p.PatientID);
            int number = 1;

            foreach (var patient in patients)
            {
                string character = !string.IsNullOrEmpty(patient.PatientName)
                    ? patient.PatientName[0].ToString().ToUpper()
                    : "?";

                string bgColor = GetColorForIndex(number);

                members.Add(new Member
                {
                    Number = number.ToString(),
                    Character = character,
                    BgColor = (Brush)converter.ConvertFromString(bgColor),
                    Name = patient.PatientName,
                    Position = patient.PatientID.ToString(),
                    Phone = patient.PatientPhone ?? "N/A"
                });

                number++;
            }

            membersDataGrid.ItemsSource = members;
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

        private void PatientSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                currentSearchType = selectedItem.Content.ToString();
                // Trigger search with new type
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    FilterPatients(txtSearch.Text);
                }
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
            {
                switch (currentSearchType)
                {
                    case "ID":
                        return m.Position.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                    case "Name":
                        return m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                    case "Phone":
                        return m.Phone.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                    default:
                        return false;
                }
            });

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

        private void Dashboard_btn(object sender, RoutedEventArgs e)
        {
            newdoctordashboard newdoctordashboard = new newdoctordashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Doctor Dashboard"
            };
            newdoctordashboard.Show();
            this.Close();
        }

        private void Report_btn(object sender, RoutedEventArgs e)
        {
            Report report = new Report
            {
                WindowState = WindowState.Normal,
                Title = "Report"
            };
            report.Show();

        }

        private void Calender_btn(object sender, RoutedEventArgs e)
        {
            Nurse_s_dashboard nurse_S_Dashboard = new Nurse_s_dashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Nurse's Dashboard"
            };
            nurse_S_Dashboard.Show();
            this.Close();
        }

        private void Logout_btn(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow
            {
                WindowState = WindowState.Maximized,
                Title = "Login"
            };
            mainWindow.Show();
            this.Close();
        }

        private void Addnewpatient_btn(object sender, RoutedEventArgs e)
        {
            NewPatient newPatient = new NewPatient
            {
                WindowState = WindowState.Normal,
                Title = "New Patient"
            };
            newPatient.Show();
        }


    }

    public class Member
    {
        public string Number { get; set; }
        public string Character { get; set; }
        public Brush BgColor { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}