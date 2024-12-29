using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for newdoctordashboard.xaml
    /// </summary>
    public partial class newdoctordashboard : Window
    {
        private readonly IdataHelper<Patient> dataHelperPatient;
        private readonly IdataHelper<Appointment> dataHelperAppointment;
        private Patient currentFirstPatient;
        private Patient currentSecondPatient;
        private Patient currentThirdPatient;

        public newdoctordashboard()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            if (services == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            dataHelperPatient = services.GetService<IdataHelper<Patient>>() ?? throw new InvalidOperationException("Patient data helper service is not available.");
            dataHelperAppointment = services.GetService<IdataHelper<Appointment>>() ?? throw new InvalidOperationException("Appointment data helper service is not available.");
            LoadTodaysAppointments();
        }

        private void LoadTodaysAppointments()
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .Take(3)
                .ToList();

            // Clear existing text
            First_patient.Text = string.Empty;
            time_patient_1.Text = string.Empty;
            currentFirstPatient = null;
            currentSecondPatient = null;
            currentThirdPatient = null;

            // Display appointments in cards
            if (todaysAppointments.Count > 0)
            {
                currentFirstPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[0].PatientID_FK);
                if (currentFirstPatient != null)
                {
                    First_patient.Text = currentFirstPatient.PatientName;
                    time_patient_1.Text = todaysAppointments[0].AppointmentTime;
                }
            }

            if (todaysAppointments.Count > 1)
            {
                currentSecondPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[1].PatientID_FK);
                if (currentSecondPatient != null)
                {
                    Second_patient.Text = currentSecondPatient.PatientName;
                    time_patient_2.Text = todaysAppointments[1].AppointmentTime;
                }
            }

            if (todaysAppointments.Count > 2)
            {
                currentThirdPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[2].PatientID_FK);
                if (currentThirdPatient != null)
                {
                    Third_patient.Text = currentThirdPatient.PatientName;
                    time_patient_3.Text = todaysAppointments[2].AppointmentTime;
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

        private void log_out(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void OpenPatientDetails(Patient patient)
        {
            if (patient != null)
            {
                IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                {
                    Title = "Patient Details",
                    WindowState = WindowState.Maximized
                };
                integRatedPatient.Show();
                this.Close();
            }
        }

        private void Upcoming_Patient_1(object sender, RoutedEventArgs e)
        {
            OpenPatientDetails(currentFirstPatient);
        }

        private void Upcoming_Patient_2(object sender, RoutedEventArgs e)
        {
            OpenPatientDetails(currentSecondPatient);
        }

        private void Upcoming_Patient_3(object sender, RoutedEventArgs e)
        {
            OpenPatientDetails(currentThirdPatient);
        }

        private void Calender_btn(object sender, RoutedEventArgs e)
        {
            Nurse_s_dashboard nurse_S_Dashboard = new Nurse_s_dashboard
            {
                Title = "Calender",
                WindowState = WindowState.Normal
            };
            nurse_S_Dashboard.Show();
            this.Close();
        }
    }
}