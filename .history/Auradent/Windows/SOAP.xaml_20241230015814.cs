using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Auradent.Windows
{
    public partial class SOAP : Window
    {
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;
        private readonly IdataHelper<Patient> _patientHelper;
        private readonly Patient _currentPatient;
        private Medical_Record _medicalRecord;

        public SOAP(Patient patient)
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();
            _patientHelper = services.GetService<IdataHelper<Patient>>();
            _currentPatient = patient;

            // Load existing medical record
            _medicalRecord = _medicalRecordHelper.GetAllData()
                .FirstOrDefault(m => m.RecordId == patient.MedicalRecordID);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var services = ((App)Application.Current).Services;
                var visitHelper = services.GetService<IdataHelper<Visit_Record>>();

                // Get new visit ID
                int visitId = visitHelper.GetAllData().Count > 0
                    ? visitHelper.GetAllData().Max(v => v.VisitId) + 1
                    : 1;

                // Build Subjective field (complaints and pain level)
                var subjectiveBuilder = new StringBuilder();
                if (ComplaintComboBox.SelectedItem is ComboBoxItem selectedComplaint)
                {
                    subjectiveBuilder.AppendLine($"Complaint: {selectedComplaint.Content}");
                    if (selectedComplaint.Content.ToString() == "Other" && OtherComplaintTextBox.IsVisible)
                    {
                        subjectiveBuilder.AppendLine($"Details: {OtherComplaintTextBox.Text}");
                    }
                }

                // Build Objective field (oral hygiene, periodontal status, initial diagnosis)
                var objectiveBuilder = new StringBuilder();
                if (InitialDiagnoseComboBox.SelectedItem is ComboBoxItem selectedDiagnosis)
                {
                    objectiveBuilder.AppendLine($"Initial Diagnosis: {selectedDiagnosis.Content}");
                    if (selectedDiagnosis.Content.ToString() == "Other" && InitialDiagnoseTextBox.IsVisible)
                    {
                        objectiveBuilder.AppendLine($"Diagnosis Details: {InitialDiagnoseTextBox.Text}");
                    }
                }

                // Create new visit record
                var visit = new Visit_Record
                {
                    VisitId = visitId,
                    PatientID_FK = _currentPatient.PatientID,
                    VisitDate = DateTime.Now,
                    Subjective = subjectiveBuilder.ToString(),
                    Objective = objectiveBuilder.ToString(),
                    Assessment = AssessmentNotes?.Text,
                    Plan = PlanNotes?.Text,
                    Notes = "",
                    Status = "Completed"
                };

                // Save to database
                if (!visitHelper.CheckIfIdExists(visit.VisitId))
                {
                    int result = visitHelper.Add(visit);
                    if (result == 1)
                    {
                        // Update medical record
                        if (_medicalRecord != null)
                        {
                            _medicalRecord.Assessment = AssessmentNotes?.Text;
                            _medicalRecord.TreatmentPlan = PlanNotes?.Text;
                            _medicalRecordHelper.Update(_medicalRecord);
                        }

                        MessageBox.Show("Visit record saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error saving visit record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error: Visit ID already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving visit record: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = this.WindowState == WindowState.Maximized ?
                    WindowState.Normal : WindowState.Maximized;
            }
        }
    }
}


