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

                PatientAgeText.Text = (DateTime.Now.Year - _patient.DateOfBirth.Year).ToString();
                PatientGenderText.Text = _patient.Gender;
                PatientPhoneText.Text = _patient.PatientPhone;
                PatientAddressText.Text = _patient.PatientAddress;

                // Update chronic diseases
                if (!string.IsNullOrEmpty(_patient.chronic_diseases))
                {
                    var diseases = _patient.chronic_diseases.Split(',')
                        .Select(d => d.Trim())
                        .ToList();
                    DiseasesItemsControl.ItemsSource = diseases;
                    ChronicDiseasesText.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DiseasesItemsControl.ItemsSource = null;
                    ChronicDiseasesText.Text = "No chronic diseases recorded";
                    ChronicDiseasesText.Visibility = Visibility.Visible;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Handle view all button click
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
            if (sender is UserControl card && card.Name == "FinanceCard")
            {
                var financeWindow = new patient_finance.Patient_Finance();
                financeWindow.SetPatient(_patient);
                financeWindow.Show();
            }
            else
            {
                SOAP sOAP = new SOAP(_patient)
                {
                    WindowState = WindowState.Normal,
                    Title = "SOAP - " + _patient.PatientName
                };
                sOAP.Show();
            }
        }

        private void FinanceBtn(object sender, RoutedEventArgs e)
        {
            Patient_Finance patient_Finance = new Patient_Finance
            {
                WindowState = WindowState.Normal,
                Title = "Patient Finance"
            };
            patient_Finance.Show();

        }
    }
}