using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Auradent.core;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Auradent.Data;
using Auradent.Windows.patient_finance;

namespace Auradent.Windows
{
    public partial class IntegRatedPatient : Window
    {
        private readonly Patient _patient;
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;
        private readonly IdataHelper<Visit_Record> _visitHelper;

        // Constructor for XAML designer
        public IntegRatedPatient()
        {
            InitializeComponent();
        }

        public IntegRatedPatient(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
            var services = ((App)Application.Current).Services;
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();
            _visitHelper = services.GetService<IdataHelper<Visit_Record>>();
            LoadPatientData();
        }

        private void LoadPatientData()
        {
            if (_patient != null)
            {
                // Load medical record
                var medicalRecord = _medicalRecordHelper.GetAllData()
                    .FirstOrDefault(m => m.RecordId == _patient.MedicalRecordID);

                if (medicalRecord != null)
                {
                    // Update Assessment Notes
                    AssessmentNotesText.Text = !string.IsNullOrEmpty(medicalRecord.Assessment)
                        ? medicalRecord.Assessment
                        : "No assessment notes available";

                    // Update Plan Notes
                    PlanNotesText.Text = !string.IsNullOrEmpty(medicalRecord.TreatmentPlan)
                        ? medicalRecord.TreatmentPlan
                        : "No treatment plan available";
                }
                else
                {
                    AssessmentNotesText.Text = "No medical record found";
                    PlanNotesText.Text = "No medical record found";
                }

                // Update other patient information
                PatientIdText.Text = _patient.PatientID.ToString();
                PatientNameText.Text = _patient.PatientName;

                // Update visit count
                var visitCount = _visitHelper.GetAllData()
                    .Count(v => v.PatientID_FK == _patient.PatientID);
                PreviousVisitsInfoCard.Value = visitCount.ToString();
                PreviousVisitsInfoCard.BottomText = $"{visitCount} visits";
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
            var newDoctorDashboard = new newdoctordashboard()
            {
                WindowState = WindowState.Maximized
            };
            newDoctorDashboard.Show();
            this.Close();
        }

        private void Medical_rec_Btn(object sender, RoutedEventArgs e)
        {
            // Handle medical records button click
            MedicalRecord secondWindow = new MedicalRecord();
            secondWindow.Show();
        }

        private void Tooth_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button toothButton)
            {
                int toothNumber = int.Parse(toothButton.Content.ToString());
                // Open the detailed tooth window with patient data
                ToothDetailWindow detailWindow = new ToothDetailWindow(_patient, toothNumber);
                detailWindow.ShowDialog();
                StatusText.Text = $"Viewing details for Tooth {toothNumber}.";
            }
        }

        private void InfoCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is UserControl.InfoCard infoCard && infoCard.Title == "Previous Visits")
            {
                var previousVisits = new PreviousVisits(_patient)
                {
                    WindowState = WindowState.Maximized
                };
                previousVisits.Show();
            }
        }

        private void FinanceBtn(object sender, RoutedEventArgs e)
        {
            var financeWindow = new Patient_Finance(_patient)
            {
                WindowState = WindowState.Maximized
            };
            financeWindow.Show();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPatientData();
            MessageBox.Show("Patient data has been refreshed.", "Refresh Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReportBtn(object sender, RoutedEventArgs e)
        {
        }
    }
}