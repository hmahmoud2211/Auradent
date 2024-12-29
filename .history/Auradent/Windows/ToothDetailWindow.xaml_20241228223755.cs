using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for ToothDetailWindow.xaml
    /// </summary>
    public partial class ToothDetailWindow : Window
    {
        private bool isEraserMode = false;
        private readonly Patient _patient;
        private readonly int _toothNumber;
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;
        private Medical_Record _medicalRecord;

        public ToothDetailWindow(Patient patient, int toothNumber)
        {
            InitializeComponent();
            _patient = patient;
            _toothNumber = toothNumber;

            var services = ((App)Application.Current).Services;
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();

            // Load existing medical record
            LoadMedicalRecord();
            LoadExistingMarks();
        }

        private void LoadMedicalRecord()
        {
            _medicalRecord = _medicalRecordHelper.GetAllData()
                .FirstOrDefault(m => m.RecordId == _patient.MedicalRecordID);

            if (_medicalRecord != null)
            {
                // Load existing notes for this tooth
                var toothNotes = GetToothNotes();
                if (!string.IsNullOrEmpty(toothNotes))
                {
                    NotesBox.Text = toothNotes;
                }
            }
        }

        private void LoadExistingMarks()
        {
            if (_medicalRecord != null)
            {
                var toothMarks = GetToothMarks();
                if (toothMarks != null)
                {
                    foreach (var mark in toothMarks)
                    {
                        Ellipse point = new Ellipse
                        {
                            Width = 10,
                            Height = 10,
                            Fill = Brushes.Red,
                            Stroke = Brushes.DarkRed,
                            StrokeThickness = 1
                        };

                        Canvas.SetLeft(point, mark.X);
                        Canvas.SetTop(point, mark.Y);
                        ToothCanvas.Children.Add(point);
                    }
                }
            }
        }

        private void ToothCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(ToothCanvas);

            if (isEraserMode)
            {
                var elementsToRemove = ToothCanvas.Children.OfType<Ellipse>()
                    .Where(ellipse => IsPointNearEllipse(clickPoint, ellipse))
                    .ToList();

                foreach (var element in elementsToRemove)
                {
                    ToothCanvas.Children.Remove(element);
                }
            }
            else
            {
                Ellipse point = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Red,
                    Stroke = Brushes.DarkRed,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(point, clickPoint.X - point.Width / 2);
                Canvas.SetTop(point, clickPoint.Y - point.Height / 2);
                ToothCanvas.Children.Add(point);
            }
        }

        private bool IsPointNearEllipse(Point clickPoint, Ellipse ellipse)
        {
            double ellipseX = Canvas.GetLeft(ellipse) + ellipse.Width / 2;
            double ellipseY = Canvas.GetTop(ellipse) + ellipse.Height / 2;

            double distance = Math.Sqrt(
                Math.Pow(clickPoint.X - ellipseX, 2) +
                Math.Pow(clickPoint.Y - ellipseY, 2)
            );

            return distance <= 15;
        }

        private void EraserToggle_Click(object sender, RoutedEventArgs e)
        {
            isEraserMode = (sender as ToggleButton)?.IsChecked ?? false;
            ToothCanvas.Cursor = isEraserMode ? Cursors.Hand : Cursors.Arrow;
        }

        private void SaveNotes_Click(object sender, RoutedEventArgs e)
        {
            if (_medicalRecord == null)
            {
                MessageBox.Show("Unable to save: Medical record not found.");
                return;
            }

            try
            {
                // Save tooth marks
                var marks = ToothCanvas.Children.OfType<Ellipse>()
                    .Select(ellipse => new ToothMark
                    {
                        X = Canvas.GetLeft(ellipse) + ellipse.Width / 2,
                        Y = Canvas.GetTop(ellipse) + ellipse.Height / 2
                    })
                    .ToList();

                SaveToothMarks(marks);
                SaveToothNotes(NotesBox.Text);

                _medicalRecordHelper.Update(_medicalRecord);
                MessageBox.Show("Changes saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}");
            }
        }

        private void SaveToothMarks(List<ToothMark> marks)
        {
            var toothData = GetOrCreateToothData();
            toothData.Marks = marks;
            UpdateToothData(toothData);
        }

        private void SaveToothNotes(string notes)
        {
            var toothData = GetOrCreateToothData();
            toothData.Notes = notes;
            UpdateToothData(toothData);
        }

        private ToothData GetOrCreateToothData()
        {
            var toothDataJson = _medicalRecord.Notes ?? "{}";
            var toothDataDict = JsonSerializer.Deserialize<Dictionary<string, ToothData>>(toothDataJson)
                              ?? new Dictionary<string, ToothData>();

            if (!toothDataDict.ContainsKey(_toothNumber.ToString()))
            {
                toothDataDict[_toothNumber.ToString()] = new ToothData();
            }

            return toothDataDict[_toothNumber.ToString()];
        }

        private void UpdateToothData(ToothData toothData)
        {
            var toothDataJson = _medicalRecord.Notes ?? "{}";
            var toothDataDict = JsonSerializer.Deserialize<Dictionary<string, ToothData>>(toothDataJson)
                              ?? new Dictionary<string, ToothData>();

            toothDataDict[_toothNumber.ToString()] = toothData;
            _medicalRecord.Notes = JsonSerializer.Serialize(toothDataDict);
        }

        private List<ToothMark> GetToothMarks()
        {
            var toothData = GetOrCreateToothData();
            return toothData.Marks;
        }

        private string GetToothNotes()
        {
            var toothData = GetOrCreateToothData();
            return toothData.Notes;
        }
    }

    public class ToothMark
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class ToothData
    {
        public List<ToothMark> Marks { get; set; } = new List<ToothMark>();
        public string Notes { get; set; } = string.Empty;
    }
}
