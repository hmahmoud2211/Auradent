using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Auradent.core;
using System.Linq;

namespace Auradent.Windows
{
    public partial class IntegRatedPatient : Window
    {
        private readonly Patient _patient;
        private readonly MedicalRecordHelper _medicalRecordHelper;

        // Constructor for XAML designer
        public IntegRatedPatient()
        {
            InitializeComponent();
        }

        public IntegRatedPatient(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
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
                PatientIdText.Text = _patient.PatientId.ToString();
                PatientNameText.Text = _patient.Name;
                PatientAgeText.Text = _patient.Age.ToString();
                PatientGenderText.Text = _patient.Gender;
                PatientPhoneText.Text = _patient.Phone;
                PatientAddressText.Text = _patient.Address;

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
            SOAP sOAP = new SOAP
            {
                WindowState = WindowState.Normal,
                Title = "SOAP",
            };
            sOAP.Show();
        }
    }
}