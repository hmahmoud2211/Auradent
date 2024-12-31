using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Windows
{
    public partial class Search : Window
    {
        private readonly IdataHelper<Patient> dataHelperPatient;
        private string currentSearchFilter = "ID"; // Default search filter

        public Search()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            if (services == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            dataHelperPatient = services.GetService<IdataHelper<Patient>>() ?? throw new InvalidOperationException("Patient data helper service is not available.");

            // Initialize the search box and load initial data
            if (txtSearch != null)
            {
                txtSearch.TextChanged += txtSearch_TextChanged;
            }
            LoadAllPatients();
        }

        private void LoadAllPatients()
        {
            var patients = dataHelperPatient.GetAllData().ToList();
            membersDataGrid.ItemsSource = patients;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch == null || PatientSearchComboBox == null) return;

            var searchText = txtSearch.Text?.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadAllPatients();
                return;
            }

            var patients = dataHelperPatient.GetAllData();
            var filteredPatients = currentSearchFilter switch
            {
                "ID" => patients.Where(p => p.PatientID.ToString().Contains(searchText)),
                "Name" => patients.Where(p => p.PatientName.Contains(searchText, StringComparison.OrdinalIgnoreCase)),
                "Phone" => patients.Where(p => p.PhoneNumber.Contains(searchText)),
                _ => patients
            };

            membersDataGrid.ItemsSource = filteredPatients.ToList();
        }

        private void PatientSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatientSearchComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                currentSearchFilter = selectedItem.Content.ToString();
                // Trigger search with new filter
                if (txtSearch != null)
                {
                    txtSearch_TextChanged(txtSearch, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None));
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            }
        }

        private void Dashboard_btn(object sender, RoutedEventArgs e)
        {
            newdoctordashboard newdoctordashboard = new newdoctordashboard
            {
                Title = "Dashboard",
                WindowState = WindowState.Maximized
            };
            newdoctordashboard.Show();
            this.Close();
        }

        private void Report_btn(object sender, RoutedEventArgs e)
        {
            Report report = new Report
            {
                Title = "Report",
                WindowState = WindowState.Normal
            };
            report.Show();
        }

        private void Calender_btn(object sender, RoutedEventArgs e)
        {
            Nurse_s_dashboard nurse_S_Dashboard = new Nurse_s_dashboard
            {
                Title = "Calender",
                WindowState = WindowState.Maximized
            };
            nurse_S_Dashboard.Show();
            this.Close();
        }

        private void Logout_btn(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Addnewpatient_btn(object sender, RoutedEventArgs e)
        {
            AddPatient addPatient = new AddPatient();
            addPatient.Show();
            this.Close();
        }

        private void membersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (membersDataGrid.SelectedItem is Patient selectedPatient)
            {
                IntegRatedPatient integRatedPatient = new IntegRatedPatient(selectedPatient)
                {
                    Title = "Patient Details - " + selectedPatient.PatientName,
                    WindowState = WindowState.Maximized
                };
                integRatedPatient.Show();
                this.Close();
            }
        }
    }
}