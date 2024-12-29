using Auradent.core;
using System;
using System.Windows;
using System.Windows.Input;

namespace Auradent.Windows
{
    public partial class IntegRatedPatient : Window
    {
        private readonly Patient currentPatient;

        public IntegRatedPatient(Patient patient)
        {
            InitializeComponent();
            currentPatient = patient ?? throw new ArgumentNullException(nameof(patient));
            LoadPatientData();
        }

        private void LoadPatientData()
        {
            // Load patient information into the UI
            if (currentPatient != null)
            {
                // Update personal information section
                PatientIDText.Text = currentPatient.PatientID.ToString();
                PatientNameText.Text = currentPatient.PatientName;
                PatientAgeText.Text = (DateTime.Now.Year - currentPatient.DateOfBirth.Year).ToString();
                ChronicDiseasesText.Text = currentPatient.chronic_diseases;
                // Add more fields as needed
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Handle view all button click
        }

        private void Medical_rec_Btn(object sender, RoutedEventArgs e)
        {
            // Handle medical records button click
        }

        private void Tooth_Click(object sender, RoutedEventArgs e)
        {
            // Handle tooth click event
        }
    }
}