using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Auradent.core;
using Auradent.Data;
using System.Windows.Controls;
using System.Windows.Media;

namespace Auradent.Windows
{
    public partial class MedicalRecord : Window
    {
        private readonly Patient currentPatient;
        private readonly RadiologyEF radiologyEF;
        private ObservableCollection<RadiologyORtest> labResults;

        public MedicalRecord(Patient patient)
        {
            InitializeComponent();
            currentPatient = patient;
            radiologyEF = new RadiologyEF();

            // Initialize collections
            labResults = new ObservableCollection<RadiologyORtest>();

            // Set patient info
            PatientId.Text = $"ID: {currentPatient.PatientID}";
            PatientName.Text = currentPatient.PatientName;

            LoadLabResults();
        }

        private void LoadLabResults()
        {
            var results = radiologyEF.GetAllData();
            foreach (var result in results)
            {
                if (result.MedicalRecordID == currentPatient.PatientID)
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

        private BitmapImage ByteArrayToBitmapImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void ViewImage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is RadiologyORtest test)
            {
                // Create a new window to display the image
                var imageWindow = new Window
                {
                    Title = "Image View",
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.ToolWindow
                };

                // Create an image control
                var image = new System.Windows.Controls.Image
                {
                    Source = ByteArrayToBitmapImage(test.Labtests),
                    Stretch = Stretch.Uniform,
                    MaxHeight = 600,
                    MaxWidth = 800
                };

                // Add the image to the window
                imageWindow.Content = image;

                // Show the window
                imageWindow.ShowDialog();
            }
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
                    byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    // Get OCR text using existing OCR functionality
                    string ocrText = await OCR.GetText(imageBytes);

                    // Create new RadiologyORtest entry
                    var newTest = new RadiologyORtest
                    {
                        MedicalRecordID = currentPatient.PatientID,
                        Labtests = imageBytes,
                        Report = ocrText
                    };

                    // Save to database
                    radiologyEF.Add_list(newTest);

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