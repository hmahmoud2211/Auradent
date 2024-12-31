using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronOcr;
using System.Windows.Controls;

namespace Auradent.Windows
{
    public partial class MedicalRecord : Window
    {
        private readonly Patient _patient;
        private readonly IdataHelper<MedicalImage> _imageHelper;
        private ObservableCollection<MedicalImageViewModel> _images;

        public class MedicalImageViewModel
        {
            public BitmapImage ImageSource { get; set; }
            public string OcrText { get; set; }
            public string UploadDate { get; set; }
        }

        public MedicalRecord(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
            var services = ((App)Application.Current).Services;
            _imageHelper = services.GetService<IdataHelper<MedicalImage>>();
            _images = new ObservableCollection<MedicalImageViewModel>();
            ImagesPanel.ItemsSource = _images;

            // Set patient info
            PatientName.Text = patient.PatientName;
            PatientId.Text = $"ID: {patient.PatientID}";

            LoadImages();
        }

        private void LoadImages()
        {
            var patientImages = _imageHelper.GetAllData().Where(img => img.PatientId == _patient.PatientID);
            _images.Clear();

            foreach (var img in patientImages)
            {
                var bitmapImage = new BitmapImage();
                using (var ms = new MemoryStream(img.ImageData))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();
                }

                _images.Add(new MedicalImageViewModel
                {
                    ImageSource = bitmapImage,
                    OcrText = img.OcrText,
                    UploadDate = img.UploadDate.ToString("dd/MM/yyyy HH:mm")
                });
            }

            NewImagesCount.Text = $"{_images.Count} images";
        }

        private async void UploadImage_Click(object sender, RoutedEventArgs e)
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
                    // Read the image file
                    var imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    // Perform OCR
                    var ocr = new IronTesseract();
                    using (var ocrInput = new OcrInput(openFileDialog.FileName))
                    {
                        var result = await ocr.ReadAsync(ocrInput);
                        var ocrText = result.Text;

                        // Save to database
                        var medicalImage = new MedicalImage
                        {
                            PatientId = _patient.PatientID,
                            ImageData = imageBytes,
                            OcrText = ocrText,
                            UploadDate = DateTime.Now,
                            ImageType = Path.GetExtension(openFileDialog.FileName)
                        };

                        _imageHelper.Add_list(medicalImage);

                        // Reload images
                        LoadImages();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}