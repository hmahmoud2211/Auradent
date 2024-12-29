using Auradent.core;
using Auradent.Data;
using Auradent.View.Usercontrols;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace Auradent.Windows
{

    public partial class NewPatient : Window
    {
        private IdataHelper<core.Patient> dataHelperPatient;
        private IdataHelper<Medical_Record> dataHelperRecord;
        Medical_Record new_record;
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyOption), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(MyOption), new PropertyMetadata(false));
        public NewPatient()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            if (services == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }

            dataHelperPatient = services.GetService<IdataHelper<core.Patient>>() ?? throw new InvalidOperationException("Data helper service is not available.");
            dataHelperRecord = services.GetService<IdataHelper<Medical_Record>>() ?? throw new InvalidOperationException("Data helper service is not available.");
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            IsSelected = true;
        }
        private void close_btn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Save_btn(object sender, RoutedEventArgs e)
        {

            try
            {
                // Validate Inputs
                if (string.IsNullOrWhiteSpace(Name_txt.textBox.Text) || string.IsNullOrWhiteSpace(phone_txt.textBox.Text) ||
                    string.IsNullOrWhiteSpace(national_id_txt.textBox.Text) || national_id_txt.textBox.Text.Length < 4)
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // Create new Patient
                core.Patient new_patient = new core.Patient
                {
                    PatientName = Name_txt.Textcontent,
                    Gender = (MaleRadioButton.IsChecked ?? false) ? "Male" : "Female",
                    PatientPhone = phone_txt.Textcontent,
                    PatientID = int.Parse(national_id_txt.Textcontent[^4..]),
                    DateOfBirth = date_txt.SelectedDate ?? DateTime.Now,
                    chronic_diseases = "ta3ban",
                    PatientAddress = "6 October",
                };

                // Create new Medical Record
                Medical_Record rec = new Medical_Record
                {
                    RecordId = int.Parse(national_id_txt.Textcontent[^4..]),
                    Subjective = "",
                    objective = "",
                    Report = "",
                    Assessment = "",
                    TreatmentPlan = "",
                    Notes = "",
                    RecordDate = DateTime.Now,
                    Patient = new_patient,
                };

                // Establish relationship
                new_patient.MedicalRecordID = rec.RecordId;
                
                if (!dataHelperPatient.CheckIfIdExists(new_patient.PatientID))
                {
                    dataHelperPatient.Add(new_patient);
                    dataHelperRecord.Add(rec);
                    MessageBox.Show("Patient added successfully.");
                }
                else
                {
                    MessageBox.Show("Patient with the same ID already exists.");
                }
                

                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void cancel_btn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}