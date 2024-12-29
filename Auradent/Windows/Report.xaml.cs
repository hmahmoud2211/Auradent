using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Windows.Xps;
using System.Windows.Media;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private readonly IdataHelper<Patient> _patientHelper;
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;
        private ObservableCollection<Patient> _patients;

        public Report()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _patientHelper = services.GetService<IdataHelper<Patient>>();
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();
            LoadPatients();
        }

        private void LoadPatients()
        {
            var patients = _patientHelper.GetAllData()
                .OrderByDescending(p => p.PatientID)
                .ToList();

            _patients = new ObservableCollection<Patient>(patients);
            patientsStackPanel.Children.Clear();

            foreach (var patient in _patients)
            {
                var patientControl = new View.Usercontrols.Patient
                {
                    Title = patient.PatientName,
                    ID = patient.PatientID.ToString(),
                    Case = GetPatientCase(patient),
                    Date = patient.DateOfBirth.ToString("dd-MM-yyyy"),
                    Height = 90,
                    Width = 921,
                    Margin = new Thickness(0, 20, 0, 0),
                    Tag = patient
                };

                patientControl.ViewReportClicked += PatientControl_ViewReportClicked;
                patientsStackPanel.Children.Add(patientControl);
            }
        }

        private string GetPatientCase(Patient patient)
        {
            var medicalRecord = _medicalRecordHelper.GetAllData()
                .FirstOrDefault(m => m.RecordId == patient.MedicalRecordID);

            return medicalRecord?.Assessment ?? "No case recorded";
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            newdoctordashboard doctorDashboard = new newdoctordashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Doctor Dashboard"
            };
            doctorDashboard.Show();
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            foreach (View.Usercontrols.Patient patientControl in patientsStackPanel.Children)
            {
                var patient = patientControl.Tag as Patient;
                if (patient != null)
                {
                    bool matches = patient.PatientName.ToLower().Contains(searchText) ||
                                 patient.PatientID.ToString().Contains(searchText);
                    patientControl.Visibility = matches ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void PatientControl_ViewReportClicked(object sender, Patient patient)
        {
            if (patient != null)
            {
                var medicalRecord = _medicalRecordHelper.GetAllData()
                    .FirstOrDefault(m => m.RecordId == patient.MedicalRecordID);

                if (medicalRecord != null)
                {
                    string report = GenerateReport(patient, medicalRecord);
                    ShowReportDialog(report, patient.PatientName);
                }
                else
                {
                    MessageBox.Show("No medical record found for this patient.", "No Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private string GenerateReport(Patient patient, Medical_Record medicalRecord)
        {
            return $@"MEDICAL REPORT
=============
Patient Information:
-------------------
Name: {patient.PatientName}
ID: {patient.PatientID}
Date of Birth: {patient.DateOfBirth:dd-MM-yyyy}
Gender: {patient.Gender}
Phone: {patient.PatientPhone}

Medical Record:
--------------
Subjective:
{medicalRecord.Subjective}

Objective:
{medicalRecord.objective}

Assessment:
{medicalRecord.Assessment}

Treatment Plan:
{medicalRecord.TreatmentPlan}

Notes:
{medicalRecord.Notes}

Report Generated: {DateTime.Now:dd-MM-yyyy HH:mm:ss}
";
        }

        private void ShowReportDialog(string report, string patientName)
        {
            var reportWindow = new Window
            {
                Title = $"Medical Report - {patientName}",
                Width = 800,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            var grid = new Grid();
            var textBox = new TextBox
            {
                Text = report,
                IsReadOnly = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(10)
            };

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10)
            };

            var printButton = new Button
            {
                Content = "Print",
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(0, 0, 10, 0)
            };
            printButton.Click += (s, e) => PrintReport(report, patientName);

            var closeButton = new Button
            {
                Content = "Close",
                Padding = new Thickness(10, 5, 10, 5)
            };
            closeButton.Click += (s, e) => reportWindow.Close();

            buttonPanel.Children.Add(printButton);
            buttonPanel.Children.Add(closeButton);

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Grid.SetRow(textBox, 0);
            Grid.SetRow(buttonPanel, 1);

            grid.Children.Add(textBox);
            grid.Children.Add(buttonPanel);

            reportWindow.Content = grid;
            reportWindow.ShowDialog();
        }

        private void PrintReport(string report, string patientName)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument doc = new FlowDocument();
                doc.PagePadding = new Thickness(50);

                Paragraph header = new Paragraph(new Run($"Medical Report - {patientName}"))
                {
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                doc.Blocks.Add(header);

                foreach (string line in report.Split('\n'))
                {
                    doc.Blocks.Add(new Paragraph(new Run(line)));
                }

                doc.PageHeight = printDialog.PrintableAreaHeight;
                doc.PageWidth = printDialog.PrintableAreaWidth;
                doc.ColumnWidth = printDialog.PrintableAreaWidth;

                IDocumentPaginatorSource idpSource = doc;
                printDialog.PrintDocument(idpSource.DocumentPaginator, $"Medical Report - {patientName}");
            }
        }
    }
}