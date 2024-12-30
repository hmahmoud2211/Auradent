using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Linq;
using System.Windows.Documents;

namespace Auradent.Windows
{
    public partial class SOAP : Window
    {
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;
        private readonly IdataHelper<Patient> _patientHelper;
        private Medical_Record _medicalRecord;
        private Patient _patient;

        public SOAP(Patient patient = null)
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();
            _patientHelper = services.GetService<IdataHelper<Patient>>();

            _patient = patient;
            LoadMedicalRecord();
        }

        private void LoadMedicalRecord()
        {
            if (_patient != null && _patient.MedicalRecordID > 0)
            {
                _medicalRecord = _medicalRecordHelper.GetAllData()
                    .FirstOrDefault(m => m.RecordId == _patient.MedicalRecordID);
            }

            if (_medicalRecord == null)
            {
                _medicalRecord = new Medical_Record
                {
                    RecordDate = DateTime.Now
                };
                _medicalRecordHelper.Add(_medicalRecord);

                // If we have a patient, update their medical record ID
                if (_patient != null)
                {
                    _patient.MedicalRecordID = _medicalRecord.RecordId;
                    _patientHelper.Update(_patient);
                }
            }

            // Populate Assessment Notes from medical record's Assessment field
            if (!string.IsNullOrEmpty(_medicalRecord.Assessment))
            {
                AssessmentNotes.Text = _medicalRecord.Assessment;
                AssessmentPlaceholder.Visibility = Visibility.Collapsed;
            }

            // Populate Plan Notes from medical record's TreatmentPlan field
            if (!string.IsNullOrEmpty(_medicalRecord.TreatmentPlan))
            {
                PlanNotes.Text = _medicalRecord.TreatmentPlan;
                PlanPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the SOAP data from the form
                var subjective = SubjectiveTextBox.Text;
                var objective = ObjectiveTextBox.Text;
                var assessment = AssessmentTextBox.Text;
                var plan = PlanTextBox.Text;

                // Find or create the PreviousVisits window
                PreviousVisits previousVisitsWindow = Application.Current.Windows.OfType<PreviousVisits>().FirstOrDefault();
                if (previousVisitsWindow == null)
                {
                    previousVisitsWindow = new PreviousVisits(_currentPatient.PatientName);
                    previousVisitsWindow.Show();
                }

                // Add the new visit record
                previousVisitsWindow.AddVisit(subjective, objective, assessment, plan);

                MessageBox.Show("SOAP record saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving SOAP record: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
            }
        }

        private void ComplaintComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComplaintComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                OtherComplaintTextBox.Visibility = selectedItem.Content.ToString() == "Other" ?
                    Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void InitialDiagnoseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InitialDiagnoseComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                InitialDiagnoseTextBox.Visibility = selectedItem.Content.ToString() == "Other" ?
                    Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void AssessmentNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            AssessmentPlaceholder.Visibility = string.IsNullOrEmpty(AssessmentNotes.Text) ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private void PlanNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlanPlaceholder.Visibility = string.IsNullOrEmpty(PlanNotes.Text) ?
                Visibility.Visible : Visibility.Collapsed;
        }
    }
}


