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
        private readonly Patient _currentPatient;
        private Medical_Record _medicalRecord;
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;
        private readonly IdataHelper<Patient> _patientHelper;

        public SOAP(Patient patient)
        {
            InitializeComponent();
            _currentPatient = patient;
            var services = ((App)Application.Current).Services;
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();
            _patientHelper = services.GetService<IdataHelper<Patient>>();
            LoadPatientData();
        }

        private void LoadPatientData()
        {
            if (_currentPatient != null && _currentPatient.MedicalRecordID > 0)
            {
                _medicalRecord = _medicalRecordHelper.GetAllData()
                    .FirstOrDefault(m => m.RecordId == _currentPatient.MedicalRecordID);
            }

            if (_medicalRecord == null)
            {
                _medicalRecord = new Medical_Record
                {
                    RecordDate = DateTime.Now
                };
                _medicalRecordHelper.Add(_medicalRecord);

                // If we have a patient, update their medical record ID
                if (_currentPatient != null)
                {
                    _currentPatient.MedicalRecordID = _medicalRecord.RecordId;
                    _patientHelper.Update(_currentPatient);
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
                subjectiveBuilder.AppendLine($"Pain Level: {PainlevelTextBox.Text}/10");

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
                if (OralHygieneComboBox.SelectedItem is ComboBoxItem selectedHygiene)
                {
                    objectiveBuilder.AppendLine($"Oral Hygiene Status: {selectedHygiene.Content}");
                }
                if (PeriodontalStatusComboBox.SelectedItem is ComboBoxItem selectedPerio)
                {
                    objectiveBuilder.AppendLine($"Periodontal Status: {selectedPerio.Content}");
                }

                // Save to medical record
                _medicalRecord.Subjective = subjectiveBuilder.ToString();
                _medicalRecord.objective = objectiveBuilder.ToString();
                _medicalRecord.Assessment = AssessmentNotes.Text;
                _medicalRecord.TreatmentPlan = PlanNotes.Text;
                _medicalRecordHelper.Update(_medicalRecord);

                // Update Patient's Chronic Diseases with Allergies
                if (_currentPatient != null && !string.IsNullOrWhiteSpace(AllergiesTextBox.Text))
                {
                    var allergies = $"Allergy: {AllergiesTextBox.Text}";
                    if (string.IsNullOrEmpty(_currentPatient.chronic_diseases))
                    {
                        _currentPatient.chronic_diseases = allergies;
                    }
                    else if (!_currentPatient.chronic_diseases.Contains("Allergy:"))
                    {
                        _currentPatient.chronic_diseases += $", {allergies}";
                    }
                    else
                    {
                        // Replace existing allergy entry
                        var parts = _currentPatient.chronic_diseases.Split(',');
                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (parts[i].Trim().StartsWith("Allergy:"))
                            {
                                parts[i] = allergies;
                                break;
                            }
                        }
                        _currentPatient.chronic_diseases = string.Join(", ", parts);
                    }
                    _patientHelper.Update(_currentPatient);
                }

                // Add to Previous Visits
                PreviousVisits previousVisitsWindow = Application.Current.Windows.OfType<PreviousVisits>().FirstOrDefault();
                if (previousVisitsWindow == null)
                {
                    previousVisitsWindow = new PreviousVisits(_currentPatient.PatientName);
                    previousVisitsWindow.Show();
                }

                // Add the visit record with current date and time
                previousVisitsWindow.AddVisit(
                    subjectiveBuilder.ToString(),
                    objectiveBuilder.ToString(),
                    AssessmentNotes.Text,
                    PlanNotes.Text
                );

                MessageBox.Show("SOAP record saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Close the SOAP window after saving
                this.Close();
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


