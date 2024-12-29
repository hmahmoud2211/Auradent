﻿using System.Windows;
using System.Windows.Input;
using Auradent.core;

namespace Auradent.Windows
{
    public partial class IntegRatedPatient : Window
    {
        private readonly Patient _patient;

        public IntegRatedPatient(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
            LoadPatientData();
        }

        private void LoadPatientData()
        {
            // Load personal information
            if (_patient != null)
            {
                // Update personal information section
                PatientIdText.Text = _patient.PatientID.ToString();
                PatientNameText.Text = _patient.PatientName;
                PatientPhoneText.Text = _patient.PatientPhone;
                PatientAddressText.Text = _patient.PatientAddress;
                PatientGenderText.Text = _patient.Gender;

                // Calculate age from date of birth
                int age = DateTime.Now.Year - _patient.DateOfBirth.Year;
                if (DateTime.Now.DayOfYear < _patient.DateOfBirth.DayOfYear)
                    age--;
                PatientAgeText.Text = age.ToString();

                // Update chronic diseases section
                ChronicDiseasesText.Text = _patient.chronic_diseases;
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
            // Handle tooth click
        }
    }
}