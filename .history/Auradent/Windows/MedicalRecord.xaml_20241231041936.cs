using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronOcr;

namespace Auradent.Windows
{
    public partial class MedicalRecord : Window
    {
        private readonly Patient _patient;
        private readonly IdataHelper<LabTest> _labTestHelper;
        private readonly string _uploadsPath;

        public MedicalRecord(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
            var services = ((App)Application.Current).Services;
            _labTestHelper = services.GetService<IdataHelper<LabTest>>();
            
            // Create uploads directory if it doesn't exist
            _uploadsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
            Directory.CreateDirectory(_uploadsPath);

            LoadPatientInfo();
            LoadLabTests();
        }

        private void LoadPatientInfo()
        {
            PatientId.Text = _patient.PatientID.ToString();
            PatientName.Text = _patient.PatientName;
            PatientDescription.Text = $"Patient ID: {_patient.PatientID}";
        }

        private void LoadLabTests()
        {
            var labTests = _labTestHelper.GetAllData()
                .Where(lt => lt.PatientID == _patient.PatientID)
                .OrderByDescending(lt => lt.UploadDate)
                .ToList();

            var labResults = labTests.Where(lt => lt.TestType == "Lab").ToList();
            var radiologyResults = labTests.Where(lt => lt.TestType == "Radiology").ToList();

            LabTestsList.ItemsSource = labResults;
            ImagesList.ItemsSource = radiologyResults;

            NewLabCount.Text = $"{labResults.Count} lab tests";
            NewImagesCount.Text = $"{radiologyResults.Count} images";
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
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

        private async void UploadLab_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Create a temporary file to store the image
                    var tempFile = Path.Combine(_uploadsPath, $"{Guid.NewGuid()}{Path.GetExtension(openFileDialog.FileName)}");
                    File.Copy(openFileDialog.FileName, tempFile);

                    // Perform OCR
                    var ocr = new IronTesseract();
                    using (var input = new OcrInput(openFileDialog.FileName))
                    {
                        var result = await ocr.ReadAsync(input);
                        var ocrText = result.Text;

                        // Save to database
                        var labTest = new LabTest
                        {
                            PatientID = _patient.PatientID,
                            ImagePath = tempFile,
                            OcrText = ocrText,
                            UploadDate = DateTime.Now,
                            TestType = "Lab"
                        };

                        _labTestHelper.Add(labTest);
                        LoadLabTests(); // Refresh the display

                        // Clean up the temporary file
                        try
                        {
                            File.Delete(tempFile);
                        }
                        catch { } // Ignore cleanup errors
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}