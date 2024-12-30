using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Auradent.Windows
{
    public partial class PreviousVisits : Window
    {
        private ObservableCollection<VisitRecord> _visits;
        private readonly string _patientName;

        public PreviousVisits(string patientName)
        {
            InitializeComponent();
            _patientName = patientName;
            PatientNameText.Text = $"- {patientName}";
            InitializeVisits();
        }

        private void InitializeVisits()
        {
            _visits = new ObservableCollection<VisitRecord>();
            VisitsListView.ItemsSource = _visits;
        }

        public void AddVisit(string subjective, string objective, string assessment, string plan)
        {
            var visit = new VisitRecord
            {
                Date = DateTime.Now.ToString("MMM dd, yyyy HH:mm"),
                Type = "SOAP Record",
                Subjective = subjective,
                Objective = objective,
                Assessment = assessment,
                Plan = plan
            };

            _visits.Insert(0, visit); // Add to the beginning of the list
            VisitsListView.SelectedItem = visit; // Select the new visit
        }

        private void VisitsListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (VisitsListView.SelectedItem is VisitRecord visit)
            {
                VisitDateText.Text = visit.Date;
                VisitTypeText.Text = visit.Type;
                SubjectiveText.Text = visit.Subjective;
                ObjectiveText.Text = visit.Objective;
                AssessmentText.Text = visit.Assessment;
                PlanText.Text = visit.Plan;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ?
                WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class VisitRecord
    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plan { get; set; }
    }
}