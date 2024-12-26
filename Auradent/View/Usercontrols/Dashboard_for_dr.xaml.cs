using Auradent.core;
using Auradent.Data;
using Auradent.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Auradent.View.Usercontrols
{
    /// <summary>
    /// Interaction logic for Dashboard_for_dr.xaml
    /// </summary>
    public partial class Dashboard_for_dr : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        private IdataHelper<Patient> dataHelperEmployee;
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public Dashboard_for_dr()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            if (services == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            dataHelperEmployee = services.GetService<IdataHelper<Patient>>() ?? throw new InvalidOperationException("Data helper service is not available.");

            Patient patient = new Patient
            {
                PatientID = 1,
                PatientName = "Biko",
                PatientAddress = "Cairo",
                PatientPhone = "01027120213",
                DateOfBirth = new DateTime(1999, 12, 12),
                Gender = "male",
                chronic_diseases = "none",

            };
            if (!dataHelperEmployee.CheckIfIdExists(1))
                dataHelperEmployee.Add(patient);
            this.First_patient.Text = patient.PatientName;
        }


        private void Upcoming_patient1(object sender, RoutedEventArgs e)
        {
            IntegRatedPatient secondWindow = new IntegRatedPatient();
            secondWindow.Show();
            Window.GetWindow(this).Close();
            void Recent_patient_1(object sender, RoutedEventArgs e)
            {
                IntegRatedPatient secondWindow = new IntegRatedPatient();
                secondWindow.Show();
                Window currentWindow = Window.GetWindow(this);
                if (currentWindow != null)
                {
                    currentWindow.Close();
                }


            }
        }
    }
}
