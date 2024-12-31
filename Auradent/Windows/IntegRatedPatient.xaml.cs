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
            MedicalRecord secondWindow = new MedicalRecord(_patient);
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

        private void ReportBtn(object sender, RoutedEventArgs e)
        {
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Simply reload all patient data
            LoadPatientData();
        }

        private void InfoCard_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            PreviousVisits previousVisits = new PreviousVisits(_patient.PatientName)
            {
                WindowState = WindowState.Normal,
                Title = "Previous Visits - " + _patient.PatientName
            };
            previousVisits.Show();

        }

        private void Dashboard_btn(object sender, RoutedEventArgs e)
        {
            newdoctordashboard newdoctordashboard = new newdoctordashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Doctor Dashboard"
            };
            newdoctordashboard.Show();
            this.Close();
        }

        private void Report_btn(object sender, RoutedEventArgs e)
        {
            Report report = new Report
            {
                WindowState = WindowState.Normal,
                Title = "Report"
            };
            report.Show();

        }

        private void Calender_btn(object sender, RoutedEventArgs e)
        {
            Nurse_s_dashboard nurse_S_Dashboard = new Nurse_s_dashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Nurse's Dashboard"
            };
            nurse_S_Dashboard.Show();
            this.Close();
        }

        private void Logout_btn(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow
            {
                WindowState = WindowState.Maximized,
                Title = "Login"
            };
            mainWindow.Show();
            this.Close();
        }

        private void Search_btn(object sender, RoutedEventArgs e)
        {
            Search search = new Search
            {
                WindowState = WindowState.Maximized,
                Title = "Search"
            };
            search.Show();
            this.Close();
        }

        private void chatbot_btn(object sender, RoutedEventArgs e)
        {
            chatbot chatbot = new chatbot
            {
                WindowState = WindowState.Normal,
                Title = "Chatbot"
            };
            chatbot.Show();
            this.Close();
        }
    }
}