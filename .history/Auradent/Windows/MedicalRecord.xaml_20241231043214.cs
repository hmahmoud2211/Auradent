using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Auradent.core;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Windows
{
    public partial class MedicalRecord : Window
    {
        private readonly Patient currentPatient;
        private readonly IdataHelper<RadiologyORtest> dataHelperRadiology;
        private ObservableCollection<RadiologyORtest> labResults;

        public MedicalRecord(Patient patient)
        {
            InitializeComponent();
            currentPatient = patient;
            var services = ((App)Application.Current).Services;
            dataHelperRadiology = services.GetService<IdataHelper<RadiologyORtest>>();
            
            // Initialize collections
            labResults = new ObservableCollection<RadiologyORtest>();
            
            // Set patient info
            PatientId.Text = $"ID: {currentPatient.PatientID}";
            PatientName.Text = currentPatient.PatientName;
            
            LoadLabResults();
        }

        private void LoadLabResults()
        {
            var results = dataHelperRadiology.GetAllData();
            foreach (var result in results)
            {
                if (result.PatientID_FK == currentPatient.PatientID)
                {
                    labResults.Add(result);
                }
            }
            
            LabResultsList.ItemsSource = labResults;
            ImagesList.ItemsSource = labResults;
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
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Read the image file
                    var imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    
                    // Get OCR text using existing OCR functionality
                    var ocrText = await OCR.GetText(imageBytes);

                    // Save image to a local folder
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(openFileDialog.FileName)}";
                    var localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName);
                    
                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                    File.Copy(openFileDialog.FileName, localPath);

                    // Create new RadiologyORtest entry
                    var newTest = new RadiologyORtest
                    {
                        PatientID_FK = currentPatient.PatientID,
                        ImagePath = localPath,
                        Report = ocrText,
                        UploadDate = DateTime.Now
                    };

                    // Save to database
                    dataHelperRadiology.Add(newTest);
                    
                    // Add to observable collection
                    labResults.Add(newTest);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}