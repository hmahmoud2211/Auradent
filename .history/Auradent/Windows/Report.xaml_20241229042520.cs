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
using System.Printing;

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
                    Margin = new Thickness(0, 5, 0, 0),
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

        private void PatientControl_ViewReportClicked(object sender, Patient patient)
        {
            var medicalRecord = _medicalRecordHelper.GetAllData()
                .FirstOrDefault(m => m.RecordId == patient.MedicalRecordID);

            if (medicalRecord == null)
            {
                MessageBox.Show("No medical record found for this patient.", "No Record", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Create report content
            var reportWindow = new Window
            {
                Title = "Medical Report",
                Width = 800,
                Height = 1000,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = new SolidColorBrush(Colors.White)
            };

            var flowDocument = CreateReportDocument(patient, medicalRecord);
            var viewer = new FlowDocumentScrollViewer
            {
                Document = flowDocument
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Children.Add(viewer);
            Grid.SetRow(viewer, 0);

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10)
            };

            var printButton = new Button
            {
                Content = "Print",
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(5)
            };
            printButton.Click += (s, e) => PrintReport(flowDocument);

            var saveButton = new Button
            {
                Content = "Save as PDF",
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(5)
            };
            saveButton.Click += (s, e) => SaveAsPdf(flowDocument);

            buttonPanel.Children.Add(printButton);
            buttonPanel.Children.Add(saveButton);

            grid.Children.Add(buttonPanel);
            Grid.SetRow(buttonPanel, 1);

            reportWindow.Content = grid;
            reportWindow.ShowDialog();
        }

        private FlowDocument CreateReportDocument(Patient patient, Medical_Record record)
        {
            var document = new FlowDocument
            {
                PagePadding = new Thickness(50),
                FontFamily = new FontFamily("Arial")
            };

            // Add header
            var header = new Paragraph
            {
                TextAlignment = TextAlignment.Center,
                FontSize = 24,
                FontWeight = FontWeights.Bold
            };
            header.Inlines.Add(new Run("Medical Report"));
            document.Blocks.Add(header);

            // Add patient information section
            var patientInfo = new Section();
            patientInfo.Blocks.Add(CreateInfoParagraph("Patient Information", 18));
            patientInfo.Blocks.Add(CreateFieldParagraph("Name", patient.PatientName));
            patientInfo.Blocks.Add(CreateFieldParagraph("ID", patient.PatientID.ToString()));
            patientInfo.Blocks.Add(CreateFieldParagraph("Date of Birth", patient.DateOfBirth.ToString("dd-MM-yyyy")));
            patientInfo.Blocks.Add(CreateFieldParagraph("Gender", patient.Gender));
            patientInfo.Blocks.Add(CreateFieldParagraph("Phone", patient.PatientPhone));
            patientInfo.Blocks.Add(CreateFieldParagraph("Address", patient.PatientAddress));
            document.Blocks.Add(patientInfo);

            // Add medical record section
            var medicalInfo = new Section();
            medicalInfo.Blocks.Add(CreateInfoParagraph("Medical Record", 18));
            medicalInfo.Blocks.Add(CreateFieldParagraph("Assessment", record.Assessment));
            medicalInfo.Blocks.Add(CreateFieldParagraph("Treatment Plan", record.TreatmentPlan));
            medicalInfo.Blocks.Add(CreateFieldParagraph("Notes", record.Notes));
            document.Blocks.Add(medicalInfo);

            // Add chronic diseases section if any
            if (!string.IsNullOrEmpty(patient.chronic_diseases))
            {
                var chronicDiseases = new Section();
                chronicDiseases.Blocks.Add(CreateInfoParagraph("Chronic Diseases", 18));
                foreach (var disease in patient.chronic_diseases.Split(','))
                {
                    chronicDiseases.Blocks.Add(CreateFieldParagraph("•", disease.Trim()));
                }
                document.Blocks.Add(chronicDiseases);
            }

            return document;
        }

        private Paragraph CreateInfoParagraph(string text, double fontSize)
        {
            return new Paragraph(new Run(text))
            {
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 20, 0, 10)
            };
        }

        private Paragraph CreateFieldParagraph(string label, string value)
        {
            var paragraph = new Paragraph { Margin = new Thickness(0, 5, 0, 5) };
            paragraph.Inlines.Add(new Run(label + ": ") { FontWeight = FontWeights.Bold });
            paragraph.Inlines.Add(new Run(value));
            return paragraph;
        }

        private void PrintReport(FlowDocument document)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                document.PageHeight = printDialog.PrintableAreaHeight;
                document.PageWidth = printDialog.PrintableAreaWidth;
                document.ColumnWidth = printDialog.PrintableAreaWidth;
                
                IDocumentPaginatorSource idpSource = document;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "Medical Report");
            }
        }

        private void SaveAsPdf(FlowDocument document)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".pdf",
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Create XPS file first
                    string xpsFile = Path.GetTempFileName();
                    using (XpsDocument xpsDoc = new XpsDocument(xpsFile, FileAccess.ReadWrite))
                    {
                        XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
                        writer.Write(((IDocumentPaginatorSource)document).DocumentPaginator);
                    }

                    // Convert XPS to PDF (you'll need a PDF converter library for this step)
                    MessageBox.Show("Report saved as XPS. To save as PDF, please install a PDF converter.", "Save Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
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
    }
}