using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System;
using Auradent.core;
using System.Windows.Documents;
using Auradent.Data;
using System.Drawing;
using Tesseract;
using System.Net.Http;

namespace Auradent.Windows
{
    public partial class MedicalRecord : Window
    {
        private RadiologyEF radiologyEF;
        private const string TESSDATA_PATH = "tessdata";
        private const string LANG_DATA_URL = "https://github.com/tesseract-ocr/tessdata/raw/main/eng.traineddata";

        public MedicalRecord()
        {
            InitializeComponent();
            radiologyEF = new RadiologyEF();
            EnsureTesseractData();
        }

        private async void EnsureTesseractData()
        {
            try
            {
                if (!Directory.Exists(TESSDATA_PATH))
                {
                    Directory.CreateDirectory(TESSDATA_PATH);
                }

                string langFile = Path.Combine(TESSDATA_PATH, "eng.traineddata");
                if (!File.Exists(langFile))
                {
                    using (var client = new HttpClient())
                    {
                        byte[] langData = await client.GetByteArrayAsync(LANG_DATA_URL);
                        await File.WriteAllBytesAsync(langFile, langData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Tesseract: {ex.Message}\nOCR functionality may not work properly.", 
                    "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void PieChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Scan_Btn(object sender, RoutedEventArgs e)
        {
            // Create an OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a Photo",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == true)
            {
                // Load the selected image into the Image control
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));

            }

        }

        private async Task<string> PerformOCR(string imagePath)
        {
            try
            {
                string tessdataPath = Path.GetFullPath(TESSDATA_PATH);
                using (var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default))
                {
                    // Set parameters for better OCR of medical documents
                    engine.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.,-+()[]{}/:;%");
                    engine.SetVariable("preserve_interword_spaces", "1");

                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            if (string.IsNullOrWhiteSpace(text))
                            {
                                throw new Exception("No text could be extracted from the image.");
                            }
                            return text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during OCR: {ex.Message}", "OCR Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        private async void UploadLab_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Lab Test Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Load the image and perform OCR
                    string extractedText = await PerformOCR(openFileDialog.FileName);
                    if (string.IsNullOrEmpty(extractedText))
                    {
                        MessageBox.Show("No text could be extracted from the image.", "OCR Result", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Create a new tab for the result
                    var tabItem = CreateLabResultTab(System.IO.Path.GetFileName(openFileDialog.FileName), extractedText);
                    LabResultsTabControl.Items.Add(tabItem);
                    LabResultsTabControl.SelectedItem = tabItem;

                    // Save to database
                    SaveLabResult(openFileDialog.FileName, extractedText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private TabItem CreateLabResultTab(string fileName, string content)
        {
            var tabItem = new TabItem
            {
                Header = fileName
            };

            var scrollViewer = new ScrollViewer();
            var stackPanel = new StackPanel { Margin = new Thickness(10) };

            // Add print button
            var printButton = new Button
            {
                Content = "Print Report",
                Style = (Style)FindResource("buttonMainGreen"),
                Margin = new Thickness(0, 0, 0, 10)
            };
            printButton.Click += (s, e) => PrintLabReport(fileName, content);
            stackPanel.Children.Add(printButton);

            // Add content
            var textBlock = new TextBlock
            {
                Text = content,
                TextWrapping = TextWrapping.Wrap
            };
            stackPanel.Children.Add(textBlock);

            scrollViewer.Content = stackPanel;
            tabItem.Content = scrollViewer;

            return tabItem;
        }

        private void PrintLabReport(string fileName, string content)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                var flowDocument = new FlowDocument();
                flowDocument.Blocks.Add(new Paragraph(new Run($"Lab Test Report: {fileName}")));
                flowDocument.Blocks.Add(new Paragraph(new Run(content)));

                var documentPaginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                printDialog.PrintDocument(documentPaginator, $"Lab Test - {fileName}");
            }
        }

        private void SaveLabResult(string imagePath, string extractedText)
        {
            try
            {
                var labTest = new RadiologyORtest
                {
                    Report = extractedText,
                    Labtests = File.ReadAllBytes(imagePath),
                    MedicalRecordID = 1 // TODO: Replace with actual medical record ID
                };

                radiologyEF.Add_list(labTest);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving lab result: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}