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

        public SOAP()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();
            _patientHelper = services.GetService<IdataHelper<Patient>>();

            LoadMedicalRecord();
        }

        private void LoadMedicalRecord()
        {
            // Get all patients with their medical records
            var patients = _patientHelper.GetAllData();
            if (patients != null && patients.Any())
            {
                // Get the most recent patient with a medical record
                _patient = patients.OrderByDescending(p => p.MedicalRecordID).FirstOrDefault();
                if (_patient != null && _patient.MedicalRecordID > 0)
                {
                    _medicalRecord = _medicalRecordHelper.GetAllData()
                        .FirstOrDefault(m => m.RecordId == _patient.MedicalRecordID);
                }
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
                _medicalRecord.Subjective = subjectiveBuilder.ToString();

                // Build Objective field (temperature, pulse rate, initial diagnosis, respiratory rate)
                var objectiveBuilder = new StringBuilder();
                if (InitialDiagnoseComboBox.SelectedItem is ComboBoxItem selectedDiagnosis)
                {
                    objectiveBuilder.AppendLine($"Initial Diagnosis: {selectedDiagnosis.Content}");
                    if (selectedDiagnosis.Content.ToString() == "Other" && InitialDiagnoseTextBox.IsVisible)
                    {
                        objectiveBuilder.AppendLine($"Diagnosis Details: {InitialDiagnoseTextBox.Text}");
                    }
                }
                objectiveBuilder.AppendLine($"Temperature: {TemperatureTextBox.Text}°C");
                objectiveBuilder.AppendLine($"Pulse Rate: {PulseRateTextBox.Text} bpm");
                objectiveBuilder.AppendLine($"Respiratory Rate: {RespRateTextBox.Text} breaths/min");
                _medicalRecord.objective = objectiveBuilder.ToString();

                // Save Notes - Get text from RichTextBox
                TextRange textRange = new TextRange(
                    ExaminationNotesTextBox.Document.ContentStart,
                    ExaminationNotesTextBox.Document.ContentEnd
                );
                _medicalRecord.Notes = textRange.Text;

                // Save Treatment Plan (current medications)
                _medicalRecord.TreatmentPlan = CurrentMedTextBox.Text;

                // Update Medical Record
                _medicalRecordHelper.Update(_medicalRecord);

                // Update Patient's Chronic Diseases with Allergies
                if (_patient != null && !string.IsNullOrWhiteSpace(AllergiesTextBox.Text))
                {
                    var allergies = $"Allergy: {AllergiesTextBox.Text}";
                    if (string.IsNullOrEmpty(_patient.chronic_diseases))
                    {
                        _patient.chronic_diseases = allergies;
                    }
                    else if (!_patient.chronic_diseases.Contains("Allergy:"))
                    {
                        _patient.chronic_diseases += $", {allergies}";
                    }
                    else
                    {
                        // Replace existing allergy entry
                        var parts = _patient.chronic_diseases.Split(',');
                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (parts[i].Trim().StartsWith("Allergy:"))
                            {
                                parts[i] = allergies;
                                break;
                            }
                        }
                        _patient.chronic_diseases = string.Join(", ", parts);
                    }
                    _patientHelper.Update(_patient);
                }

                MessageBox.Show("Data saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
