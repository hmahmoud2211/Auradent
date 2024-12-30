using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Windows.Xps;
using Auradent.core;
using System.Windows.Markup;

namespace Auradent.Windows
{
    public partial class PatientReportWindow : Window
    {
        private readonly Patient _patient;
        private readonly Medical_Record _medicalRecord;

        public PatientReportWindow(Patient patient, Medical_Record medicalRecord)
        {
            InitializeComponent();
            _patient = patient;
            _medicalRecord = medicalRecord;
            LoadReportData();
        }

        private void LoadReportData()
        {
            // Patient Information
            PatientNameText.Text = _patient.PatientName;
            PatientIdText.Text = _patient.PatientID.ToString();
            GenderText.Text = _patient.Gender;
            PhoneText.Text = _patient.PatientPhone;

            // Medical Record
            SubjectiveText.Text = _medicalRecord.Subjective ?? "Not recorded";
            ObjectiveText.Text = _medicalRecord.objective ?? "Not recorded";
            AssessmentText.Text = _medicalRecord.Assessment ?? "Not recorded";
            PlanText.Text = _medicalRecord.TreatmentPlan ?? "Not recorded";

            // Chronic Diseases
            ChronicDiseasesText.Text = !string.IsNullOrEmpty(_patient.chronic_diseases)
                ? _patient.chronic_diseases
                : "No chronic diseases recorded";

            // Report Date
            ReportDateText.Text = $"Report generated on {DateTime.Now:MMMM dd, yyyy HH:mm}";
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create the print dialog
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Create a document for printing
                    FixedDocument document = new FixedDocument();

                    // Create a page for the document
                    var pageContent = new FixedPage();
                    pageContent.Width = printDialog.PrintableAreaWidth;
                    pageContent.Height = printDialog.PrintableAreaHeight;

                    // Clone the report content for printing
                    var contentToprint = new FrameworkElement();
                    contentToprint = ReportContent;

                    // Add the content to the page
                    pageContent.Children.Add(contentToprint);

                    // Create a new page content and add it to the document
                    var page = new PageContent();
                    ((IAddChild)page).AddChild(pageContent);
                    document.Pages.Add(page);

                    // Print the document
                    printDialog.PrintDocument(document.DocumentPaginator, "Patient Medical Report");

                    MessageBox.Show("Report printed successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing report: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}