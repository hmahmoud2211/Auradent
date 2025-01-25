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
using IronOcr;

namespace Auradent.Windows
{
    public partial class MedicalRecord : Window
    {
        private readonly Patient currentPatient;
        private readonly RadiologyEF radiologyEF;
        private ObservableCollection<RadiologyORtest> labResults;
        private readonly IronTesseract _ocr;
        private ObservableCollection<ScanItem> _scans;

        public MedicalRecord(Patient patient)
        {
            InitializeComponent();
            currentPatient = patient;
            radiologyEF = new RadiologyEF();
            
            // Initialize OCR
            _ocr = new IronTesseract();
            _ocr.Language = OcrLanguage.English;
            
            // Initialize collections
            _scans = new ObservableCollection<ScanItem>();

            // Set patient info
            PatientId.Text = $"ID: {currentPatient.PatientID}";
            PatientName.Text = currentPatient.PatientName;

            // Set the scans list source
            ScansList.ItemsSource = _scans;
        }

        private void LoadLabResults()
        {
            try
            {
                var results = radiologyEF.GetAllData();
                foreach (var result in results)
                {
                    if (result.MedicalRecordID == currentPatient.PatientID)
                    {
                        labResults.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading lab results: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void UploadScan_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    var scan = new ScanItem
                    {
                        Id = Guid.NewGuid(),
                        ImageSource = new BitmapImage(new Uri(openFileDialog.FileName)),
                        ScanDate = DateTime.Now,
                        FilePath = openFileDialog.FileName
                    };
                    _scans.Add(scan);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading scan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ProcessOCR_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Guid scanId)
            {
                var scan = _scans.FirstOrDefault(s => s.Id == scanId);
                if (scan != null)
                {
                    try
                    {
                        using (var input = new OcrInput(scan.FilePath))
                        {
                            input.Deskew();  // Fix image rotation
                            input.DeNoise(); // Remove noise
                            
                            var result = _ocr.Read(input);
                            OcrResultsText.Text = result.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"OCR Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SaveResults_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OcrResultsText.Text))
            {
                MessageBox.Show("No OCR results to save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt",
                FileName = $"OCR_Results_{currentPatient.PatientID}_{DateTime.Now:yyyyMMdd}"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, OcrResultsText.Text);
                    MessageBox.Show("Results saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving results: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class ScanItem
    {
        public Guid Id { get; set; }
        public BitmapImage ImageSource { get; set; }
        public DateTime ScanDate { get; set; }
        public string FilePath { get; set; }
    }
}