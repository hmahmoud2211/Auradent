using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Windows.Xps;
using Microsoft.Extensions.DependencyInjection;
using Auradent.core;
using Auradent.Data;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private readonly IdataHelper<Patient> _patientHelper;
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;

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
            var patients = _patientHelper.GetAllData();
            PatientsGrid.ItemsSource = patients;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            var filteredPatients = _patientHelper.GetAllData()
                .Where(p => p.PatientName.ToLower().Contains(searchText) ||
                           p.PatientID.ToString().Contains(searchText) ||
                           p.PatientPhone.ToLower().Contains(searchText))
                .ToList();
            PatientsGrid.ItemsSource = filteredPatients;
        }

        private void ViewReport_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Patient patient)
            {
                var medicalRecord = _medicalRecordHelper.GetAllData()
                    .FirstOrDefault(m => m.RecordId == patient.MedicalRecordID);

                if (medicalRecord != null)
                {
                    var reportWindow = new PatientReportWindow(patient, medicalRecord);
                    reportWindow.Show();
                }
                else
                {
                    MessageBox.Show("No medical record found for this patient.", "Information",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}