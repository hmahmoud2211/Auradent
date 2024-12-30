﻿using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for newdoctordashboard.xaml
    /// </summary>
    public partial class newdoctordashboard : Window
    {
        private readonly IdataHelper<Patient> dataHelperPatient;
        private readonly IdataHelper<Appointment> dataHelperAppointment;

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
            LoadUpcomingPatients();
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

            // Display appointments in cards
            if (todaysAppointments.Count > 0)
            {
                var firstPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[0].PatientID_FK);
                if (firstPatient != null)
                {
                    First_patient.Text = firstPatient.PatientName;
                    time_patient_1.Text = todaysAppointments[0].AppointmentTime;
                }
            }

            if (todaysAppointments.Count > 1)
            {
                var secondPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[1].PatientID_FK);
                if (secondPatient != null)
                {
                    Second_patient.Text = secondPatient.PatientName;
                    time_patient_2.Text = todaysAppointments[1].AppointmentTime;
                }
            }

            if (todaysAppointments.Count > 2)
            {
                var thirdPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[2].PatientID_FK);
                if (thirdPatient != null)
                {
                    Third_patient.Text = thirdPatient.PatientName;
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

        private void Upcoming_Patient_1(object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .FirstOrDefault();

            if (todaysAppointments != null)
            {
                var patient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments.PatientID_FK);
                if (patient != null)
                {
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                    {
                        Title = "Patient Details - " + patient.PatientName,
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                    this.Close();
                }
            }
        }

        private void Calender_btn(object sender, RoutedEventArgs e)
        {
            Nurse_s_dashboard nurse_S_Dashboard = new Nurse_s_dashboard
            {
                Title = "Calender",
                WindowState = WindowState.Maximized
            };
            nurse_S_Dashboard.Show();
            this.Close();
        }

        private void Upcoming_Patient_2(object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .Skip(1)
                .FirstOrDefault();

            if (todaysAppointments != null)
            {
                var patient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments.PatientID_FK);
                if (patient != null)
                {
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                    {
                        Title = "Patient Details - " + patient.PatientName,
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                    this.Close();
                }
            }
        }

        private void Upcoming_Patient_3(object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .Skip(2)
                .FirstOrDefault();

            if (todaysAppointments != null)
            {
                var patient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments.PatientID_FK);
                if (patient != null)
                {
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                    {
                        Title = "Patient Details - " + patient.PatientName,
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                    this.Close();
                }
            }
        }

        private void SearchBtn(object sender, RoutedEventArgs e)
        {
            Search search = new Search
            {
                Title = "Search",
                WindowState = WindowState.Maximized
            };
            search.Show();
            this.Close();

        }

        private void ReportBtn(object sender, RoutedEventArgs e)
        {
            Report report = new Report
            {
                Title = "Report",
                WindowState = WindowState.Normal
            };
            report.Show();
        }

        public void LoadUpcomingPatients()
        {
            var today = DateTime.Today;
            var appointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date >= today)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .Take(3)
                .ToList();

            // Clear existing text
            First_patient.Text = "";
            Second_patient.Text = "";
            Third_patient.Text = "";
            time_patient_1.Text = "";
            time_patient_2.Text = "";
            time_patient_3.Text = "";

            for (int i = 0; i < appointments.Count; i++)
            {
                var appointment = appointments[i];
                var patient = dataHelperPatient.GetAllData()
                    .FirstOrDefault(p => p.PatientID == appointment.PatientID_FK);

                if (patient != null)
                {
                    switch (i)
                    {
                        case 0:
                            First_patient.Text = patient.PatientName;
                            time_patient_1.Text = $"{appointment.AppointmentDate.ToShortDateString()} {appointment.AppointmentTime}";
                            break;
                        case 1:
                            Second_patient.Text = patient.PatientName;
                            time_patient_2.Text = $"{appointment.AppointmentDate.ToShortDateString()} {appointment.AppointmentTime}";
                            break;
                        case 2:
                            Third_patient.Text = patient.PatientName;
                            time_patient_3.Text = $"{appointment.AppointmentDate.ToShortDateString()} {appointment.AppointmentTime}";
                            break;
                    }
                }
            }
        }
    }
}