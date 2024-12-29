using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private readonly IdataHelper<Patient> _patientHelper;
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;
        private ObservableCollection<Patient> _patients;

        public Report()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _patientHelper = services.GetService<IdataHelper<Patient>>();
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();
            LoadPatients();
        }

        private void LoadPatients()
        {
            var patients = _patientHelper.GetAllData()
                .OrderByDescending(p => p.PatientID)
                .ToList();

            _patients = new ObservableCollection<Patient>(patients);
            patientsStackPanel.Children.Clear();

            foreach (var patient in _patients)
            {
                var patientControl = new View.Usercontrols.Patient
                {
                    Title = patient.PatientName,
                    ID = patient.PatientID.ToString(),
                    Case = GetPatientCase(patient),
                    Date = patient.DateOfBirth.ToString("dd-MM-yyyy"),
                    Height = 90,
                    Width = 921,
                    Margin = new Thickness(0, 20, 0, 0),
                    Tag = patient
                };

                patientControl.ViewReportClicked += PatientControl_ViewReportClicked;
                patientsStackPanel.Children.Add(patientControl);
            }
        }

        private string GetPatientCase(Patient patient)
        {
            var medicalRecord = _medicalRecordHelper.GetAllData()
                .FirstOrDefault(m => m.RecordId == patient.MedicalRecordID);
            
            return medicalRecord?.Assessment ?? "No case recorded";
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            newdoctordashboard doctorDashboard = new newdoctordashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Doctor Dashboard"
            };
            doctorDashboard.Show();
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            
            foreach (View.Usercontrols.Patient patientControl in patientsStackPanel.Children)
            {
                var patient = patientControl.Tag as Patient;
                if (patient != null)
                {
                    bool matches = patient.PatientName.ToLower().Contains(searchText) ||
                                 patient.PatientID.ToString().Contains(searchText);
                    patientControl.Visibility = matches ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }
}